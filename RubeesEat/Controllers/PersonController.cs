using Microsoft.AspNetCore.Mvc;
using RubeesEat.Model;

namespace RubeesEat.Controllers;

[ApiController]
[Route("/api/persons")]
public class PersonController(IPersonRepository personRepository) : ControllerBase
{
    [Route("")]
    [HttpGet]
    public IEnumerable<Person> GetAll()
    {
        return personRepository.GetAll();
    }
}
