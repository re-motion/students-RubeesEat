using System.Data;
using System.Data.Common;
using MySqlConnector;
using RubeesEat.Model;
using RubeesEat.Model.DB;

namespace ExcelToDBTransferer;

public class DbTransferer
{
    private readonly DbConnection _connection;
    private readonly DbPersonRepository _personRepository;
    private readonly DbBillRepository _billRepository;
    private readonly IDictionary<string, PersonDTO> _personsToBeAdded = new Dictionary<string, PersonDTO>();
    private readonly IDictionary<int, Bill> _billsToBeAdded = new Dictionary<int, Bill>();

    public DbTransferer(IDbConnectionFactory connectionFactory, DbPersonRepository personRepository, DbBillRepository billRepository)
    {
        _connection = connectionFactory.CreateDbConnection();
        _personRepository = personRepository;
        _billRepository = billRepository;
    }
    
    public List<PersonDTO> TransferToDb(List<PersonDTO>? persons)
    {
        foreach (var person in persons!)
            _personsToBeAdded.Add(person.Initials, person);
        using var selectCommand = _connection.CreateCommand("SELECT Initials, PersonID FROM Excel_Persons");
        _connection.Open();
        using var selectReader = selectCommand.ExecuteReader();
        while (selectReader.Read())
        {
            var initials = selectReader.GetString(0);
            foreach(var person in persons)
                if (person.Initials == initials)
                    person.Id = selectReader.GetGuid(1);
            _personsToBeAdded.Remove(initials);
        }
        _connection.Close();
        if (_personsToBeAdded.Count <= 0)
            return persons;
        _connection.Open();
        
        foreach (var personDto in _personsToBeAdded.Values)
        {
            var person = new Person(personDto.Id, personDto.FirstName, personDto.LastName, personDto.IsActive);
            _personRepository.Add(person);
            using var insertCommand = _connection.CreateCommand("INSERT INTO Excel_Persons (PersonID, Initials) VALUES (@PersonID, @Initials);");
            insertCommand.AddParameter("@PersonID", personDto.Id);
            insertCommand.AddParameter("@Initials", personDto.Initials);
            insertCommand.ExecuteNonQuery();
        }
        _connection.Close();
        return persons;
    }

    public void TransferToDb(List<Bill> bills)
    {
        int row = 4;
        foreach (var bill in bills!)
        {
            _billsToBeAdded.Add(row, bill);
            row++;
        }
        using var selectCommand = _connection.CreateCommand("SELECT MAX(Row) AS MaxRow FROM Excel_Bills;");
        _connection.Open();
        using var reader = selectCommand.ExecuteReader();
        if (reader.Read() && !reader.IsDBNull(0))
        {
            var billsToRemove = _billsToBeAdded.Keys
                                               .Where(key => key <= reader.GetInt32(0))
                                               .ToList();
            
            foreach (var key in billsToRemove)
            {
                _billsToBeAdded.Remove(key);
            }
        }
        _connection.Close();
        if (_billsToBeAdded.Count <= 0)
            return;
        _connection.Open();
        

        foreach (var rowNumber in _billsToBeAdded)
        {
            _billRepository.Add(rowNumber.Value);
            using var insertCommand = _connection.CreateCommand("INSERT INTO Excel_Bills (BillID, Row) VALUES (@BillID, @Row);");
            insertCommand.AddParameter("@BillID", rowNumber.Value.Id);
            insertCommand.AddParameter("@Row", rowNumber.Key);
            insertCommand.ExecuteNonQuery();
        }
    }
}
