using System.Collections.Immutable;
using NUnit.Framework.Constraints;
using RubeesEat.Model;

namespace RubeesEat.UnitTests.Model;

[TestFixture]
public class BillTests
{
    [Test]
    public void Constructor_AssignsPropertiesCorrectly()
    {
        Guid expectedGuid = Guid.NewGuid();
        DateTime expectedTime = DateTime.Now;
        ImmutableArray<EntryLine> entryLines =
        [
            new EntryLine(new Person(Guid.NewGuid(), "Max", "Ackermann"), 100),
            new EntryLine(new Person(Guid.NewGuid(), "Minecraft", "Steve"), -50),
            new EntryLine(new Person(Guid.NewGuid(), "Schmoes", "Relax"), -50)
        ];
        
        Bill bill = new Bill(expectedGuid, expectedTime, "Going to Macdonal", entryLines);
        
        Assert.That(bill.Id, Is.EqualTo(expectedGuid));
        Assert.That(bill.Date, Is.EqualTo(expectedTime));
        Assert.That(bill.Description, Is.EqualTo("Going to Macdonal"));
        Assert.That(bill.EntryLines.Length, Is.EqualTo(3));
    }

    [Test]
    public void Constructor_WithSumNotZero_ThrowsException()
    {
        ImmutableArray<EntryLine> entryLines =
        [
            new EntryLine(new Person(Guid.NewGuid(), "Schmoes", "Relax"), 100),
            new EntryLine(new Person(Guid.NewGuid(), "Klara", "Malernachzahler"), -50)
        ];

        Assert.That(() => new Bill(Guid.NewGuid(), DateTime.Now, "Bolognese Flade in der Fladerei", entryLines),
            Throws.TypeOf<ArgumentException>().With.Message.EqualTo("Sum of entry lines should be 0."));
    }

    [Test]
    public void Constructor_WithLessThanTwoEntryLines_ThrowsException()
    {
        ImmutableArray<EntryLine> entryLines =
        [
            new EntryLine(new Person(Guid.NewGuid(), "Doctor", "Doofenshmirtz"), 100)
        ];

        Assert.That(() => new Bill(Guid.NewGuid(), DateTime.Now, "Alleine bei seiner Geburt", entryLines),
            Throws.TypeOf<ArgumentException>().With.Message.EqualTo("Entry lines should contain at least 2."));
    }
}
