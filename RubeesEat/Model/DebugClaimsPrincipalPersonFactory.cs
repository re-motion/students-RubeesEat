using System.Security.Claims;

namespace RubeesEat.Model;

public class DebugClaimsPrincipalPersonFactory : IClaimsPrincipalPersonFactory
{
    public string GetLoginName(ClaimsPrincipal claimsPrincipal)
    {
        return "patrick.widener";
    }

    public Person CreatePerson(ClaimsPrincipal claimsPrincipal, string loginName)
    {
        return new Person(
            Guid.NewGuid(),
            "Patrick",
            "Wiedener",
            "patrick.widener",
            "patrick.widener@test.com");
    }
}
