using System.Net;
using Newtonsoft.Json;
using RubeesEat.Model;

namespace RubeesEat.IntegrationTests.Controllers;

[TestFixture]
public class BillControllerTest : IntegrationTestBase
{
    [Test]
    public async Task? Create_DoesCreate()
    {
        var resultPersons = await HttpClient.GetStringAsync("/api/persons");
        var persons = JsonConvert.DeserializeObject<Person[]>(resultPersons)!;
        var resultBills = await HttpClient.GetStringAsync("/api/bills");
        var bills = JsonConvert.DeserializeObject<Bill[]>(resultBills)!;
        var url = "/api/bills";
        var content = new FormUrlEncodedContent(new[]
        {
            new KeyValuePair<string, string>("billDescription", "test234"),
            new KeyValuePair<string, string>("billDate", "2024-10-29"),
            new KeyValuePair<string, string>("billAmount", "10"),

            new KeyValuePair<string, string>("id0", persons[0].Id.ToString()),
            new KeyValuePair<string, string>("amount0", "2"),

            new KeyValuePair<string, string>("id1", persons[1].Id.ToString()),
            new KeyValuePair<string, string>("amount1", "3"),

            new KeyValuePair<string, string>("id2", persons[2].Id.ToString()),
            new KeyValuePair<string, string>("amount2", "5")
        });
        var response = await HttpClient.PostAsync(url, content);

        var resultBillsAfter = await HttpClient.GetStringAsync("/api/bills");
        var billsAfter = JsonConvert.DeserializeObject<Bill[]>(resultBillsAfter)!;

        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Redirect));
        Assert.That(billsAfter.Length, Is.EqualTo(bills.Length + 1));
        Assert.That(billsAfter[^1].Description, Is.EqualTo("test234"));
        Assert.That(billsAfter[^1].Date.ToString(), Is.EqualTo("29/10/2024 00:00:00"));
    }

    [Test]
    public async Task Update_DoesUpdate()
    {
        var resultPersons = await HttpClient.GetStringAsync("/api/persons");
        var persons = JsonConvert.DeserializeObject<Person[]>(resultPersons)!;
        var resultBills = await HttpClient.GetStringAsync("/api/bills");
        var bills = JsonConvert.DeserializeObject<Bill[]>(resultBills)!;

        var existingBill = bills[0];
        var url = $"/api/bills/{existingBill.Id}";
        var newDescription = "Updated Description";
        
        var content = new FormUrlEncodedContent(new[]
        {
            new KeyValuePair<string, string>("billId", existingBill.Id.ToString()),
            new KeyValuePair<string, string>("billDescription", newDescription),
            new KeyValuePair<string, string>("billDate", DateTime.Today.ToString("yyyy-MM-dd")),
            new KeyValuePair<string, string>("billAmount", "20.00"),
            
            new KeyValuePair<string, string>("id0", persons[0].Id.ToString()),
            new KeyValuePair<string, string>("amount0", "10.00"), 
            
            new KeyValuePair<string, string>("id1", persons[1].Id.ToString()),
            new KeyValuePair<string, string>("amount1", "10.00"),
        });

        var response = await HttpClient.PostAsync(url, content);

        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Found));

        var resultBillsAfter = await HttpClient.GetStringAsync("/api/bills");
        var billsAfter = JsonConvert.DeserializeObject<Bill[]>(resultBillsAfter)!;
        var updatedBill = billsAfter.FirstOrDefault(b => b.Id == existingBill.Id);

        Assert.That(updatedBill.Description, Is.EqualTo(newDescription));
        Assert.That(updatedBill.EntryLines[0].Amount, Is.EqualTo(20.00));
        Assert.That(updatedBill.EntryLines[1].Amount, Is.EqualTo(-10.00));
    }

    
    [Test]
    public async Task? Create_DuplicatePerson_BadRequest()
    {
        var resultPersons = await HttpClient.GetStringAsync("/api/persons");
        var persons = JsonConvert.DeserializeObject<Person[]>(resultPersons)!;
        var resultBills = await HttpClient.GetStringAsync("/api/bills");
        var bills = JsonConvert.DeserializeObject<Bill[]>(resultBills)!;
        var url = "/api/bills";
        var content = new FormUrlEncodedContent(new[]
        {
            new KeyValuePair<string, string>("billDescription", "test234"),
            new KeyValuePair<string, string>("billAmount", "10"),

            new KeyValuePair<string, string>("id0", persons[0].Id.ToString()),
            new KeyValuePair<string, string>("amount0", "2"),

            new KeyValuePair<string, string>("id1", persons[0].Id.ToString()),
            new KeyValuePair<string, string>("amount1", "3"),

            new KeyValuePair<string, string>("id2", persons[0].Id.ToString()),
            new KeyValuePair<string, string>("amount2", "5")
        });
        var response = await HttpClient.PostAsync(url, content);
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
    }
    
    [Test]
    public async Task? Create_FirstEntryNegative_BadRequest()
    {
        var resultPersons = await HttpClient.GetStringAsync("/api/persons");
        var persons = JsonConvert.DeserializeObject<Person[]>(resultPersons)!;
        var resultBills = await HttpClient.GetStringAsync("/api/bills");
        var bills = JsonConvert.DeserializeObject<Bill[]>(resultBills)!;
        var url = "/api/bills";
        var content = new FormUrlEncodedContent(new[]
        {
            new KeyValuePair<string, string>("billDescription", "test234"),
            new KeyValuePair<string, string>("billAmount", "-10"),

            new KeyValuePair<string, string>("id0", persons[0].Id.ToString()),
            new KeyValuePair<string, string>("amount0", "2"),

            new KeyValuePair<string, string>("id1", persons[0].Id.ToString()),
            new KeyValuePair<string, string>("amount1", "3"),

            new KeyValuePair<string, string>("id2", persons[0].Id.ToString()),
            new KeyValuePair<string, string>("amount2", "5")
        });
        var response = await HttpClient.PostAsync(url, content);
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
    }
    
    [Test]
    public async Task? Create_EntryAfterFirstNegative_BadRequest()
    {
        var resultPersons = await HttpClient.GetStringAsync("/api/persons");
        var persons = JsonConvert.DeserializeObject<Person[]>(resultPersons)!;
        var resultBills = await HttpClient.GetStringAsync("/api/bills");
        var bills = JsonConvert.DeserializeObject<Bill[]>(resultBills)!;
        var url = "/api/bills";
        var content = new FormUrlEncodedContent(new[]
        {
            new KeyValuePair<string, string>("billDescription", "test234"),
            new KeyValuePair<string, string>("billAmount", "-10"),

            new KeyValuePair<string, string>("id0", persons[0].Id.ToString()),
            new KeyValuePair<string, string>("amount0", "-2"),

            new KeyValuePair<string, string>("id1", persons[0].Id.ToString()),
            new KeyValuePair<string, string>("amount1", "-3"),

            new KeyValuePair<string, string>("id2", persons[0].Id.ToString()),
            new KeyValuePair<string, string>("amount2", "5")
        });
        var response = await HttpClient.PostAsync(url, content);
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
    }
}
