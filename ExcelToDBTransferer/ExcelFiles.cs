using System.Data;
using System.Text;
using ExcelDataReader;
using System.Collections.Immutable;
using System.Globalization;
using RubeesEat.Model;

namespace ExcelToDBTransferer
{
    public static class ExcelFiles
    {
        public static List<Bill> GenerateBillsFromFile(string filePath,  List<PersonDTO> listOfPersons)
        {
            var dataTable = ReadExcelFile(filePath);
            if (dataTable != null)
                return CreateBillsFromDataTable(dataTable, listOfPersons);

            Console.WriteLine("There is no data in the Excel file.");
            return new List<Bill>();
        }

        private static DataTable? ReadExcelFile(string filePath)
        {
            using var stream = File.Open(filePath, FileMode.Open, FileAccess.Read);
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            var encoding = Encoding.GetEncoding("UTF-8");

            using var reader = ExcelReaderFactory.CreateReader(stream,
                new ExcelReaderConfiguration { FallbackEncoding = encoding });
            var result = reader.AsDataSet(new ExcelDataSetConfiguration
            {
                ConfigureDataTable = _ => new ExcelDataTableConfiguration { UseHeaderRow = false }
            });

            return result.Tables.Count > 0 ? result.Tables[0] : null;
        }
        
        private static Bill CreateBillForCarryOver(DataTable dataTable,  List<PersonDTO> listOfPersons)
        {
            var carryOvers = GetEntryLinesForBill(dataTable, 3, listOfPersons);
            var voidPerson = FindPersonByInitials("VoidPerson", listOfPersons);
            decimal voidEntryValue = carryOvers.Sum(e => e.Amount);
            var voidEntry = new EntryLine(voidPerson, voidEntryValue);
            for(int i = 0; i < carryOvers.Count; i++)
                carryOvers[i] = carryOvers[i].With(carryOvers[i].Amount);
            carryOvers.Add(voidEntry);
            var billForCarryOvers = Bill.Create("All carry overs", carryOvers.ToArray());
            return billForCarryOvers;
        }
        
        private static List<Bill> CreateBillsFromDataTable(DataTable dataTable, List<PersonDTO> listOfPersons)
        {
            var bills = new List<Bill>();
            bills.Add(CreateBillForCarryOver(dataTable, listOfPersons));
            int invalidBillsCount = 0;
            for (int rowIndex = 5; rowIndex < dataTable.Rows.Count-1; rowIndex++)
            {
                DataRow row = dataTable.Rows[rowIndex];
                
                if (IsRowEmpty(row))
                {
                    continue; 
                }
                
                var description = row[2]?.ToString();
                var dateString = row[1]?.ToString();
                dateString = dateString.Replace('.', '/');
                DateTime date = DateTime.MinValue;
                if (dateString != null)
                    date = DateTime.ParseExact(dateString, "dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture);

                var entryLines = GetEntryLinesForBill(dataTable, rowIndex, listOfPersons).ToImmutableArray();
                if (entryLines.Length < 2) continue;
                if (description == null) continue;

                var bill = new Bill(Guid.NewGuid(), date, description, entryLines.ToImmutableArray());
                bills.Add(bill);
            }

            Console.WriteLine($"Number of invalid Bills detected: {invalidBillsCount}");
            return bills;
        }
        
        private static bool IsRowEmpty(DataRow row)
        {
            foreach (var item in row.ItemArray)
            {
                if (item != null && !string.IsNullOrWhiteSpace(item.ToString()))
                {
                    return false; 
                }
            }
            return true; 
        }
        
        private static List<EntryLine> GetEntryLinesForBill(DataTable dataTable, int rowIndex,  List<PersonDTO> listOfPersons)
        {
            DataRow row = dataTable.Rows[rowIndex];
            DataRow headerRow = dataTable.Rows[0]; 
            var entryLines = new List<EntryLine>();
            

            var firstPersonInitials = row[0]?.ToString()?.Trim();
            var firstPerson = FindPersonByInitials(firstPersonInitials, listOfPersons);
            string firstAmountString = row[3]?.ToString()?.Trim();
            firstAmountString = firstAmountString.Replace(',', '.');

            if (!string.IsNullOrWhiteSpace(firstAmountString) &&
                decimal.TryParse(firstAmountString, NumberStyles.Any, CultureInfo.InvariantCulture, out var firstAmount))
            {
                if (firstPerson != null)
                {
                    entryLines.Add(new EntryLine(firstPerson, firstAmount)); 
                }
            }
            else
            {
                Console.WriteLine($"Could not parse '{firstAmountString}' as a decimal. Found at row {rowIndex + 1}.");
            }

            for (int i = 5; i < dataTable.Columns.Count; i++)
            {
                string headerValue = headerRow[i]?.ToString()?.Trim(); 
                string cellValue = row[i]?.ToString()?.Trim(); 
                cellValue = cellValue.Replace(',', '.');


                if (!string.IsNullOrWhiteSpace(cellValue))
                {
                    if (decimal.TryParse(cellValue, NumberStyles.Any, CultureInfo.InvariantCulture, out var amount))
                    {
                        var person = FindPersonByInitials(headerValue, listOfPersons);
                        if (person != null)
                        {
                            entryLines.Add(new EntryLine(person, -amount)); 
                        }
                    }
                    else
                    {
                        Console.WriteLine($"Could not parse '{cellValue}' as a decimal.");
                    }
                }
            }
            decimal correctFirstAmount = -entryLines
                                         .Skip(1)
                                         .Sum(e => e.Amount);
            entryLines[0] = entryLines[0].With(correctFirstAmount);

            return entryLines;
        }
        
        private static Person? FindPersonByInitials(string initials,  List<PersonDTO> listOfPersons)
        {
            foreach (var person in listOfPersons)
            {
                if (string.Equals(person.Initials, initials, StringComparison.OrdinalIgnoreCase))
                    return person.MapToPerson();
            }
            return null;
        }
    }
}
