using System;
using System.Threading.Tasks;
using AutoMapper;
using CoreCodeCamp.Data;
using CoreCodeCamp.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore.Internal;

namespace CoreCodeCamp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CampsController : ControllerBase
    {
        private readonly ICampRepository _campRepository;
        private readonly IMapper _mapper;
        private readonly LinkGenerator _linkGenerator;
        public CampsController(ICampRepository campRepository, IMapper mapper, LinkGenerator linkGenerator)
        {
            _campRepository = campRepository;
            _mapper = mapper;
            _linkGenerator = linkGenerator;
        }

        //[HttpGet]
        //public IActionResult Get()
        //{
        //   // if (false) return BadRequest("Bad stuff happens");
        //    return Ok(new { Moniker = "ATL2020", Name = "Atlanta Code Camp" });
        //}

        [HttpGet]
        public async Task<ActionResult<CampModel[]>> Get(bool includeTalks = false)
        {
            try
            {
                // get camps from database
                Camp[] camps = await _campRepository.GetAllCampsAsync(includeTalks);

                return _mapper.Map<CampModel[]>(camps);
            }
            catch (Exception)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Database Failure");
            }

        }

        [HttpGet("{moniker}")]
        public async Task<ActionResult<CampModel>> Get(string moniker, bool includeTalks = false)
        {
            try
            {
                Camp camp = await _campRepository.GetCampAsync(moniker, includeTalks);

                if (camp == null) return NotFound();

                return _mapper.Map<CampModel>(camp);
            }
            catch (Exception)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Database Failure");

            }
        }

        [HttpGet("search/{search}")]
        public async Task<ActionResult<CampModel[]>> SearchByDate(DateTime theDate, bool includeTalks = false)
        {
            try
            {
                Camp[] camps = await _campRepository.GetAllCampsByEventDate(theDate, includeTalks);

                if (!camps.Any()) return NotFound();

                return _mapper.Map<CampModel[]>(camps);
            }
            catch (Exception)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Database Failure");
            }
        }

        [HttpPost]
        public async Task<ActionResult<CampModel>> Post(CampModel campModel)
        {
            //adds a new camp and return the location and data
            try
            {
                // Get path of HttpGet
                var location = _linkGenerator.GetPathByAction("Get",
                   "Camps", new { moniker = campModel.Moniker });

                if (string.IsNullOrWhiteSpace(location))
                {
                    return BadRequest("Could not use current moniker");
                }

                Camp camp = _mapper.Map<Camp>(campModel);
                _campRepository.Add(camp);

                if(await _campRepository.SaveChangesAsync())
                {
                    return Created("", _mapper.Map<CampModel>(camp));
                }
             
                return Ok();
            }
            catch (Exception e)
            {
               return this.StatusCode(StatusCodes.Status500InternalServerError, "Database Failure"); 
            }

        }
   
    }
}