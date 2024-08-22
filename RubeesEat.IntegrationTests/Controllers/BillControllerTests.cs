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
    }
}
