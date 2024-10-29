using System.Collections.Immutable;
using System.Globalization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using RubeesEat.Model;

namespace RubeesEat.Controllers;

[ApiController]
[Route("/api/bills")]
public class BillController(IBillRepository billRepository, IPersonRepository personRepository)
    : ControllerBase
{
    [Route("")]
    [HttpGet]
    public IEnumerable<Bill> GetAll()
    {
        return billRepository.GetAll();
    }

    [Route("")]
    [HttpPost]
    public Results<BadRequest<string>, RedirectHttpResult> Create()
    {
        var form = HttpContext.Request.Form;
        string? description = form["billDescription"];
        CultureInfo culture = new CultureInfo("en-EN");
        if (description == null)
        {
            return TypedResults.BadRequest("Description was empty.");
        }

        if (!DateTime.TryParseExact(form["billDate"], "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime date))
        {
            return TypedResults.BadRequest("Invalid date format. Date must be in yyyy-MM-dd format.");
        }

        List<EntryLine> entryLines = [];
        if (!decimal.TryParse(form["billAmount"], culture, out decimal totalAmount))
        {
            return TypedResults.BadRequest("Total amount must be a number.");
        }
        entryLines.Add(new EntryLine(personRepository.GetOrCreateUser(HttpContext.User), totalAmount));

        for (var i = 0;; i++)
        {
            string? id = form["id" + i];

            if (id == null)
            {
                break;
            }
            
            if (!Guid.TryParse((id), out var personGuid))
            {
                return TypedResults.BadRequest("Person Guid not valid");
            }
            
            var person = personRepository.GetById(personGuid);
            if (person == null)
            {
                return TypedResults.BadRequest("Person not found");
            }

            var stringValues = form["amount" + i];

            if (!decimal.TryParse(stringValues, culture,out var amount))
            {
                return TypedResults.BadRequest("Amount of a person must be a number.");
            }
            
            entryLines.Add(new EntryLine(person, -amount));
        }
        
        if (entryLines[0].Amount <= 0)
        {
            return TypedResults.BadRequest("First entry should always be positive");
        }

        for(int i = 1; i < entryLines.Count; i++)
        {
            if (entryLines[i].Amount >= 0)
            {
                return TypedResults.BadRequest("Entries after the first one should always be negative");
            }
        }
        
        int linesAfterFirst = entryLines.Skip(1).DistinctBy(p => p.Person).Count();
        int distinctLinesAfterFirst = entryLines.Skip(1).Count();
        
        if (linesAfterFirst != distinctLinesAfterFirst)
        {
            return TypedResults.BadRequest("Persons should not appear multiple times (other than the buyer)");
        }

        var bill = Bill.Create(date, description, entryLines.ToArray());

        billRepository.Add(bill);

        return TypedResults.Redirect("/");
    }
    
    [Route("{billId:guid}")]
    [HttpPost]
    
    public Results<BadRequest<string>, RedirectHttpResult> Update()
    {
        var form = HttpContext.Request.Form;
        string? description = form["billDescription"];
        var date = DateTime.Parse(form["billDate"]);
        var existingBill = billRepository.GetById(Guid.Parse(form["billId"]));
        CultureInfo culture = new CultureInfo("en-EN");

        if (existingBill == null)
        {
            return TypedResults.BadRequest("Bill not found");
        }
        
        if (description == null)
        {
            return TypedResults.BadRequest("Description was empty.");
        }

        List<EntryLine> entryLines = [];
        if (!decimal.TryParse(form["billAmount"], culture, out decimal totalAmount))
        {
            return TypedResults.BadRequest("Total amount must be a number.");
        }
        entryLines.Add(new EntryLine(personRepository.GetOrCreateUser(HttpContext.User), totalAmount));

        for (var i = 0;; i++)
        {
            string? id = form["id" + i];

            if (id == null)
            {
                break;
            }
            
            if (!Guid.TryParse((id), out var personGuid))
            {
                return TypedResults.BadRequest("Person Guid not valid");
            }
            
            var person = personRepository.GetById(personGuid);
            if (person == null)
            {
                return TypedResults.BadRequest("Person not found");
            }

            var stringValues = form["amount" + i];

            if (!decimal.TryParse(stringValues, culture,out var amount))
            {
                return TypedResults.BadRequest("Amount of a person must be a number.");
            }
            
            entryLines.Add(new EntryLine(person, -amount));
        }
        
        if (entryLines[0].Amount <= 0)
        {
            return TypedResults.BadRequest("First entry should always be positive");
        }

        for(int i = 1; i < entryLines.Count; i++)
        {
            if (entryLines[i].Amount >= 0)
            {
                return TypedResults.BadRequest("Entries after the first one should always be negative");
            }
        }
        
        int linesAfterFirst = entryLines.Skip(1).DistinctBy(p => p.Person).Count();
        int distinctLinesAfterFirst = entryLines.Skip(1).Count();
        
        if (linesAfterFirst != distinctLinesAfterFirst)
        {
            return TypedResults.BadRequest("Persons should not appear multiple times (other than the buyer)");
        }

        var bill = new Bill(existingBill.Id, date, description, entryLines.ToImmutableArray());

        billRepository.Update(bill);

        return TypedResults.Redirect("/Index");
    }
}
