using RubeesEat.Model;

namespace RubeesEat.UnitTests.Model;

[TestFixture]
public class PersonTest
{
    [Test]
    public void Constructor_AssignsPropertiesCorrectly()
    {
        Guid expectedGuid = Guid.NewGuid();
        
        var person = new Person(expectedGuid, "Levi", "Muster", "levi.muster");
        
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
            () => new Person(Guid.NewGuid(), name, name, "loginName"),
            Throws.TypeOf<ArgumentException>()
                  .With.Message.EqualTo("Name must only consist of letters."));
    }

    [Test]
    public void Constructor_WithInactiveAndLoginName_ThrowsException()
    {
        Assert.That(
            () => new Person(Guid.NewGuid(), "Test", "Test", "loginName", false),
            Throws.TypeOf<ArgumentException>()
                  .With.Message.EqualTo("Inactive users should have no login name"));
    }
    
    [Test]
    public void Constructor_WithActiveAndNoLoginName_ThrowsException()
    {
        Assert.That(
            () => new Person(Guid.NewGuid(), "Test", "Test", null),
            Throws.TypeOf<ArgumentException>()
                  .With.Message.EqualTo("Active users should have a login name"));
    }
}
