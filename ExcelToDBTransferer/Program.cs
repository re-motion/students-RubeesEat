using System.Data.Common;
using System.Net;
using System.Text.Json.Nodes;
using ExcelToDBTransferer;
using Newtonsoft.Json;
using RubeesEat.Model;
using RubeesEat.Model.DB;

public class Program
{
    public static void Main(string[] args)
    {
        if (args.Length != 3) {
            Console.WriteLine("Usage: <excelFile> <jsonFile> <connectionString>");
            Environment.Exit(1);
        }
        var excelFilePath = args[0];
        var jsonFilePath = args[1];
        var connectionString = args[2];

        string jsonContent = File.ReadAllText(jsonFilePath);
        var listOfPersons = JsonConvert.DeserializeObject<List<PersonDTO>>(jsonContent);

        var sqlConnectionFactory = new SqlConnectionFactory(connectionString);
        var personRepository = new DbPersonRepository(sqlConnectionFactory);
        var billRepository = new DbBillRepository(sqlConnectionFactory);

        var transferer = new DbTransferer(sqlConnectionFactory, personRepository, billRepository);
        var correctListOfPersons = transferer.TransferToDb(listOfPersons);
        var listOfBills = ExcelFiles.GenerateBillsFromFile(excelFilePath, correctListOfPersons);
        transferer.TransferToDb(listOfBills);
    }
}
