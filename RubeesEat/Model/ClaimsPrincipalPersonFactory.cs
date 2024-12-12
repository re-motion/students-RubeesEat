using System.Net.Mail;
using System.Security.Claims;

namespace RubeesEat.Model;

public class ClaimsPrincipalPersonFactory : IClaimsPrincipalPersonFactory
{
    const string c_domain = "RUBICON\\";

    public string GetLoginName(ClaimsPrincipal claimsPrincipal)
    {
        var loginNameWithDomain = claimsPrincipal.FindFirst("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name")?.Value;
        if (loginNameWithDomain == null)
            throw new InvalidOperationException("Cannot find the required claims on the user.");
        if (!loginNameWithDomain.StartsWith(c_domain))
            throw new InvalidOperationException("Invalid user account. Not in domain.");

        return loginNameWithDomain[c_domain.Length..];
    }

    public Person CreatePerson(ClaimsPrincipal claimsPrincipal, string loginName)
    {
        var email = claimsPrincipal.FindFirst("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/upn")?.Value;
        Console.WriteLine($"Person '{loginName}' does not exist. Creating with email '{email}'");

        if (email == null)
            throw new InvalidOperationException("Cannot create account. No Email found.");
        if (!MailAddress.TryCreate(email, out _))
            throw new InvalidOperationException($"Cannot create account. Invalid Email '{email}'.");

        var nameParts = email.Split('@')[0].Split('.');

        return new Person(
            Guid.NewGuid(),
            ToPascalCase(nameParts[0]),
            nameParts.Length > 0 ? ToPascalCase(nameParts[1]) : "",
            loginName,
            email);
    }

    private string ToPascalCase(string value)
    {
        if (value.Length == 0)
            return value;

        return char.IsUpper(value[0])
            ? value
            : char.ToUpper(value[0]) + value[1..];
    }
}
