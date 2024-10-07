using RubeesEat.Model;

namespace ExcelToDBTransferer;

public class PersonDTO
{
    public Guid Id { get; set; }
    public string FirstName { get; }
    public string LastName { get; }
    public string Initials { get;}
    public bool IsActive { get; }

    public PersonDTO(string firstName, string lastName, string initials, bool isActive)
    {
        Id = Guid.NewGuid();
        FirstName = firstName;
        LastName = lastName;
        Initials = initials;
        IsActive = isActive;
    }
    
    public Person MapToPerson()
    {
        return new Person(Id, FirstName, LastName);
    }
}
