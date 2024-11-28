using System.Collections.Immutable;
using System.Data;
using System.Data.Common;
using MySqlConnector;

namespace RubeesEat.Model.DB;

public class DbBillRepository(IDbConnectionFactory connectionFactory) : IBillRepository
{
    public IReadOnlyList<Bill> GetAll()
    {
        using var connection = connectionFactory.CreateDbConnection();
        using var command = connection.CreateCommand(
            $"""
               SELECT
                     B.BillID,
                     B.Date,
                     B.Description,
                     E.EntryLineID,
                     E.Amount,
                     P.PersonID,
                     P.FirstName,
                     P.LastName,
                     P.LoginName,
                     P.Email,
                     P.IsActive
                 FROM
                     Bills B
                 INNER JOIN
                     EntryLines E ON B.BillID = E.BillID
                  INNER JOIN
                     Persons P ON P.PersonID = E.PersonID
                 ORDER BY B.Date DESC, B.BillID;
             """);
        connection.Open();
        using var reader = command.ExecuteReader();
        return CreateBillsFromReader(reader);
    }

    public void Add(Bill bill)
    {
        using (var connection = connectionFactory.CreateDbConnection())
        {
            connection.Open();
            using var transaction = (MySqlTransaction)connection.BeginTransaction(IsolationLevel.ReadCommitted);
            {
                var command = transaction.CreateCommand(
                    "INSERT INTO Bills (BillID, Date, Description) VALUES (@BillID, @Date, @Description);")!;
                command.AddParameter("@BillID", bill.Id);
                command.AddParameter("@Date", bill.Date);
                command.AddParameter("@Description", bill.Description);
                command.ExecuteNonQuery();

                for (var i = 0; i < bill.EntryLines.Length; i++)
                {
                    var entryLine = bill.EntryLines[i];
                    var entryLineCommand = transaction.CreateCommand(
                        $"INSERT INTO EntryLines (Amount, PersonID, BillID) VALUES (@Amount{i}, @PersonID{i}, @EntryLineBillID{i})")!;
                    entryLineCommand.AddParameter($"@Amount{i}", entryLine.Amount);
                    entryLineCommand.AddParameter($"@PersonID{i}", entryLine.Person.Id);
                    entryLineCommand.AddParameter($"@EntryLineBillID{i}", bill.Id);
                    entryLineCommand.ExecuteNonQuery();
                }
                
                transaction.Commit();
            }
        }
    }

    public void Update(Bill bill)
    {
        using (var connection = connectionFactory.CreateDbConnection())
        {
            connection.Open();
            
            GetById(bill.Id);
            
            using var transaction = (MySqlTransaction)connection.BeginTransaction(IsolationLevel.ReadCommitted);
            {
                var command = transaction.CreateCommand(
                    $"UPDATE Bills SET Description = @Description, Date = @Date WHERE BillID = @BillId;")!;
                command.AddParameter("@BillID", bill.Id);
                command.AddParameter("@Description", bill.Description);
                command.AddParameter("@Date", bill.Date);
                command.ExecuteNonQuery();

                var deleteEntryLineCommand = transaction.CreateCommand(
                    $"DELETE FROM EntryLines WHERE BillID = @BillID;")!;
                deleteEntryLineCommand.AddParameter("@BillID", bill.Id);
                deleteEntryLineCommand.ExecuteNonQuery();


                for (var i = 0; i < bill.EntryLines.Length; i++)
                {
                    var entryLine = bill.EntryLines[i];
                    var entryLineCommand = transaction.CreateCommand(
                        $"INSERT INTO EntryLines (Amount, PersonID, BillID) VALUES (@Amount{i}, @PersonID{i}, @EntryLineBillID{i})")!;
                    entryLineCommand.AddParameter($"@Amount{i}", entryLine.Amount);
                    entryLineCommand.AddParameter($"@PersonID{i}", entryLine.Person.Id);
                    entryLineCommand.AddParameter($"@EntryLineBillID{i}", bill.Id);
                    entryLineCommand.ExecuteNonQuery();
                }
                
                transaction.Commit();
            }
        }
    }
    
    public IReadOnlyList<Bill> GetAllForUser(Person user)
    {
        using var connection = connectionFactory.CreateDbConnection();
        using var command = connection.CreateCommand($"""
                                                      SELECT 
                                                      	B.BillID,
                                                      	B.Date,
                                                      	B.Description,
                                                      	E.EntryLineID,
                                                      	E.Amount,
                                                      	P.PersonID,
                                                      	P.FirstName,
                                                      	P.LastName,
                                                      	P.LoginName,
                                                        P.Email,
                                                        P.IsActive
                                                      FROM
                                                      	Bills B
                                                      INNER JOIN 
                                                      	EntryLines E ON B.BillID = E.BillID
                                                      INNER JOIN
                                                      	Persons P ON P.PersonID = E.PersonID
                                                      WHERE 
                                                      	B.BillID IN (
                                                      	SELECT DISTINCT B2.BillID
                                                      	FROM Bills B2
                                                      	INNER JOIN EntryLines E2 ON B2.BillID = E2.BillID
                                                      	WHERE E2.PersonID = @PersonID
                                                      	)
                                                      ORDER BY
                                                      	B.Date DESC, B.BillID;
                                                      """);
        command.AddParameter("@PersonID", user.Id);
        connection.Open();
        using var reader = command.ExecuteReader();
        return CreateBillsFromReader(reader);
    }

    public PaginatedView<BalanceChange> GetRecentBalanceChanges(Person currentUser, int page, int pageSize)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(pageSize);
        
        var skip = (page - 1) * pageSize;
        using var connection = connectionFactory.CreateDbConnection();
        using var command = connection.CreateCommand($"""
                                                        SELECT 
                                                          SUM(el.Amount) AS TotalAmount, 
                                                          b.Date AS Date,
                                                          b.Description AS Description,
                                                          b.BillID AS BillID,
                                                          COUNT(*) OVER () AS TotalEntryLines
                                                      FROM 
                                                          EntryLines el
                                                      JOIN
                                                          Bills b ON el.BillID = b.BillID
                                                      WHERE 
                                                          el.PersonID = @PersonID
                                                      GROUP BY 
                                                          b.BillID, b.Date, b.Description
                                                      ORDER BY 
                                                          b.Date DESC
                                                      LIMIT @Skip, @Take;
                                                      """);
        command.AddParameter("@PersonID", currentUser.Id);
        command.AddParameter("@Skip", skip);
        command.AddParameter("@Take", pageSize);

        connection.Open();

        using var reader = command.ExecuteReader();

        var balanceChanges = new List<BalanceChange>();
        int totalNumberBalanceChanges  = 0;
        while (reader.Read())
        {
            balanceChanges.Add(new BalanceChange
            (
                reader.GetDecimal(0),
                reader.GetDateTime(1),
                reader.GetString(2),
                reader.GetGuid(3)
            ));
            totalNumberBalanceChanges  = reader.GetInt32(4);
        }
        int totalPages = (int)Math.Ceiling(totalNumberBalanceChanges  / (double)pageSize);

        return new PaginatedView<BalanceChange>(
            [..balanceChanges],
            page,
            totalPages
        );
    }

    public decimal GetBalance(Person user)
    {
        using var connection = connectionFactory.CreateDbConnection();
        using var command = connection.CreateCommand("SELECT SUM(Amount) FROM EntryLines WHERE PersonID = @PersonID;");
        command.AddParameter("@PersonID", user.Id);

        connection.Open();

        var result = command.ExecuteScalar();
        
        if (result == DBNull.Value)
        {
            return 0m;
        }

        return Convert.ToDecimal(result);
    }

    public Bill GetById(Guid guid)
    {
        using var connection = connectionFactory.CreateDbConnection();
        using var command = connection.CreateCommand();

        command.CommandText = $"""
                                 SELECT
                                       B.BillID,
                                       B.Date,
                                       B.Description,
                                       E.EntryLineID,
                                       E.Amount,
                                       P.PersonID,
                                       P.FirstName,
                                       P.LastName,
                                       P.LoginName,
                                       P.Email,
                                       P.IsActive
                                   FROM
                                       Bills B
                                   INNER JOIN
                                       EntryLines E ON B.BillID = E.BillID
                                    INNER JOIN
                                       Persons P ON P.PersonID = E.PersonID
                                   WHERE 
                                       B.BillID = @BillID;
                               """;        
        command.AddParameter("@BillID", guid);
        connection.Open();
        using var reader = command.ExecuteReader();
        var bills = CreateBillsFromReader(reader);
    
        if (bills.Count == 0)
        {
            return null;
        }

        return bills[0];    
    }
    
    private IReadOnlyList<Bill> CreateBillsFromReader(DbDataReader reader)
    {
        var bills = new List<Bill>();
        
        BillBuilder billBuilder = new BillBuilder();
        billBuilder.Id = Guid.Empty;    //initializes before reading data
        
        while (reader.Read())
        {
            var newBillId = reader.GetGuid(0); 
            if (billBuilder.Id != newBillId)
            {
                AddBillIfNecessary();
                billBuilder.Id = newBillId;
                billBuilder.Date = reader.GetDateTime(1);
                billBuilder.Description = reader.GetString(2);
            }
            
            var person = new Person(
                reader.GetGuid(5),
                reader.GetString(6),
                reader.GetString(7),
                reader.IsDBNull(8) ? null : reader.GetString(8),
                reader.IsDBNull(9) ? null : reader.GetString(9),
                reader.GetBoolean(10)
            );

            var entryLine = new EntryLine(
                person,
                reader.GetDecimal(4)
            );

            billBuilder.EntryLines.Add(entryLine);
        }
        
        AddBillIfNecessary();
        
        return bills.ToImmutableArray();

        void AddBillIfNecessary()
        {
            if (billBuilder.Id == Guid.Empty) return;
            bills.Add(billBuilder.Build());
            billBuilder.EntryLines.Clear();
        }
    }
}
