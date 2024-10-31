using RubeesEat.IntegrationTests.PageObjects;

namespace RubeesEat.IntegrationTests.SeleniumTests;

public class EditBillSeleniumTests() : WithTestBills("Index")
{
    [Test]
    public void UpdateDescription()
    {
        var page = Start<UserHomepagePageObject>();
        var balanceChanges = page.GetBalanceChanges();
        var balanceChange = balanceChanges[0];
        using var billDetailPage = balanceChange.ClickBillDetails();
        
        Assert.That(billDetailPage.Description, Is.EqualTo("Default user paid lunch 2025"));

        using var editBillPage = billDetailPage.ClickEdit();
        editBillPage.SetNewDesciptionText("New description");
        editBillPage.ClickUpdateBill();

        var updatedBalanceChanges = page.GetBalanceChanges();
        var updatedBalanceChange = updatedBalanceChanges[0];
        using var updatedBillDetailPage = updatedBalanceChange.ClickBillDetails();
        
        Assert.That(updatedBillDetailPage.Description, Is.EqualTo("New description"));
    }

    [Test]
    public void UpdateTotalAmount()
    {
        var page = Start<UserHomepagePageObject>();
        var balanceChanges = page.GetBalanceChanges();
        var balanceChange = balanceChanges[0];

        using var billDetailPage = balanceChange.ClickBillDetails();
        
        Assert.That(billDetailPage.Amount, Is.EqualTo("3 000,00 €"));
        
        using var editBillPage = billDetailPage.ClickEdit();
        var person = editBillPage.ClickAddPerson();
        editBillPage.SetNewTotalPriceText("3010");
        person.SetAmountForPerson("10");
        editBillPage.ClickUpdateBill();
        
        var updatedBalanceChanges = page.GetBalanceChanges();
        var updatedBalanceChange = updatedBalanceChanges[0];
        using var updatedBillDetailPage = updatedBalanceChange.ClickBillDetails();
        
        Assert.That(updatedBillDetailPage.Amount, Is.EqualTo("3 010,00 €"));
    }
}