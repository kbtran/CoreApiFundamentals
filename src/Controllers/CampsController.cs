using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoreCodeCamp.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CoreCodeCamp.Controllers
{
    [Route("api/[controller]")]
    public class CampsController : ControllerBase
    {
        private readonly ICampRepository _campRepository;

        public CampsController(ICampRepository campRepository)
        {
            _campRepository = campRepository;
        }

        //[HttpGet]
        //public IActionResult Get()
        //{
        //   // if (false) return BadRequest("Bad stuff happens");
        //    return Ok(new { Moniker = "ATL2020", Name = "Atlanta Code Camp" });
        //}

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                // get camps from database
                var camps = await _campRepository.GetAllCampsAsync();

                return Ok(camps);
            }
            catch
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Database Failure");
            }
            
        }
    }
}