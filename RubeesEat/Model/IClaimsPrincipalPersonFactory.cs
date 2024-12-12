using System.Security.Claims;

namespace RubeesEat.Model;

public interface IClaimsPrincipalPersonFactory
{
    string GetLoginName(ClaimsPrincipal claimsPrincipal);

    Person CreatePerson(ClaimsPrincipal claimsPrincipal, string loginName);
}
