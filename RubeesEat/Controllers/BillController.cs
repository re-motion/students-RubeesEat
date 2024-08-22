using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
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

        if (description == null)
        {
            return TypedResults.BadRequest("Description was empty.");
        }

        List<EntryLine> entryLines = [];
        decimal totalAmount;
        if (!decimal.TryParse(form["billAmount"], out totalAmount))
        {
            return TypedResults.BadRequest("Total amount must be a number.");
        }
        entryLines.Add(new EntryLine(personRepository.GetCurrentUser(), totalAmount));

        for (var i = 0;; i++)
        {
            string? id = form["id" + i];

            if (id == null)
            {
                break;
            }

            Person person;
            try
            {
                person = personRepository.GetById(Guid.Parse(id));
            }
            catch (KeyNotFoundException)
            {
                return TypedResults.BadRequest("User was not found.");
            }
            
            var stringValues = form["amount" + i].FirstOrDefault()!.Replace(".", ",");
            
            decimal amount;
            if (!decimal.TryParse(stringValues, out amount))
            {
                return TypedResults.BadRequest("Amount of a person must be a number.");
            }
            amount = -amount;
            
            entryLines.Add(new EntryLine(person, amount));
        }

        var bill = Bill.Create(description, entryLines.ToArray());

        billRepository.Add(bill);

        return TypedResults.Redirect("/SplitBill");
    }
}
