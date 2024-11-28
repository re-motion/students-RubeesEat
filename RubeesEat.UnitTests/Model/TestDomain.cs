using System.Collections.Immutable;
using RubeesEat.Model;

namespace RubeesEat.UnitTests.Model;

public static class TestDomain
{
    public static readonly Person Jack = Person.Create("Jack", "Frost", "jack.frost", "jack.frost@bla");
    public static readonly Person Erwin = Person.Create("Erwin", "Smith", "erwin.smith", "erwin.smith@bla");
    public static readonly Person Sasha = Person.Create("Sasha", "Braus", "sasha.braus", "sasha.braus@bla");
    public static readonly Person Eren = Person.Create("Eren", "Jaeger", "eren.jaeger", "eren.jaeger@bla");
    public static readonly Person Mikasa = Person.Create("Mikasa", "Ackermann", "mikasa.ackermann", "mikasa.ackermann@bla");
    public static readonly Person Levi = Person.Create("Levi", "Ackermann", "levi.ackermann", "levi.ackermann@bla");
    public static readonly Person Yusuke = Person.Create("Yusuke", "Kitagawa", "yusuke.kitagawa", "yusuke.kitagawa@bla");

    public static readonly List<Person> AllPeople =
    [
        Jack,
        Erwin,
        Sasha,
        Eren,
        Mikasa,
        Levi,
        Yusuke
    ];

    public static readonly Bill BillCafeLeBlanc;
    public static readonly Bill BillMaidCafe;
    public static readonly List<Bill> AllBills = [];

    static TestDomain()
    {
        List<EntryLine> entryLines =
        [
            new EntryLine(Yusuke, 50),
            new EntryLine(Jack, -50)
        ];
        BillCafeLeBlanc = Bill.Create("Curry and coffee at Le Blanc's", entryLines.ToArray());
        AllBills.Add(BillCafeLeBlanc);
        
        entryLines.Clear();
        entryLines =
        [
            new EntryLine(Mikasa, 100),
            new EntryLine(Sasha, -20),
            new EntryLine(Eren, -80)
        ];
        BillMaidCafe = Bill.Create("Normal coffee in a normal cafe", entryLines.ToArray());
        AllBills.Add(BillMaidCafe);
    }
}
