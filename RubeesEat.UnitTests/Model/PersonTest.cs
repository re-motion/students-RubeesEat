using RubeesEat.Model;

namespace RubeesEat.UnitTests.Model;

[TestFixture]
public class PersonTest
{
    [Test]
    public void Constructor_AssignsPropertiesCorrectly()
    {
        Guid expectedGuid = Guid.NewGuid();
        
        var person = new Person(expectedGuid, "Levi", "Muster", "levi.muster", "levi.muster@bla");
        
        Assert.That(person.Id, Is.EqualTo(expectedGuid));
        Assert.That(person.FirstName, Is.EqualTo("Levi"));
        Assert.That(person.LastName, Is.EqualTo("Muster"));
    }
}
