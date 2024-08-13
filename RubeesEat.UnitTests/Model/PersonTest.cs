using RubeesEat.Model;

namespace RubeesEat.UnitTests.Model;

[TestFixture]
public class PersonTest
{
    [Test]
    public void Constructor_AssignsPropertiesCorrectly()
    {
        Guid expectedGuid = Guid.NewGuid();
        
        var person = new Person(expectedGuid, "Levi", "Muster");
        
        Assert.That(person.Id, Is.EqualTo(expectedGuid));
        Assert.That(person.FirstName, Is.EqualTo("Levi"));
        Assert.That(person.LastName, Is.EqualTo("Muster"));
    }

    [Test]
    [TestCase("Test2")]
    [TestCase("3Test")]
    [TestCase("?Test")]
    [TestCase("1Test1")]
    [TestCase("Te st")]
    [TestCase(".")]
    [TestCase("")]
    [TestCase(" ")]
    public void Constructor_WithInvalidName_ThrowsException(string name)
    {
        Assert.That(
            () => new Person(Guid.NewGuid(), name, name),
            Throws.TypeOf<ArgumentException>()
                  .With.Message.EqualTo("Name must only consist of letters."));
    }
}
