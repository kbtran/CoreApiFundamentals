using Microsoft.AspNetCore.Mvc;

namespace CoreCodeCamp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ValuesController
    {
        [HttpGet]
        public string[] Get()
        {
          return new[] { "Hello", "From","Pluralsight" };
        }
    }
}
