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
            // Adds a new camp and return the location and data
            try
            {
                Camp existingCamp = await _campRepository.GetCampAsync(campModel.Moniker);
                
                if (existingCamp != null)
                {
                    return BadRequest("Moniker already in use");
                }

                // Get path of HttpGet
                string location = _linkGenerator.GetPathByAction("Get", "Camps", new { moniker = campModel.Moniker });

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
            }
            catch (Exception)
            {
               return this.StatusCode(StatusCodes.Status500InternalServerError, "Database Failure"); 
            }

            return BadRequest();
        }


        [HttpPut("{moniker}")]
        public async Task<ActionResult<CampModel>> Put(string moniker, CampModel campModel)
        {
            try
            {
                // Update a camp
                Camp existingCamp = await _campRepository.GetCampAsync(moniker);
                if (existingCamp == null) return NotFound($"Could not find the camp with moniker {moniker}");

                // CampId and LocationId are keys in their respective table so they can't be modified.
                int existingCampId = existingCamp.CampId;
                int existingLocationId = existingCamp.Location.LocationId;

                _mapper.Map(campModel, existingCamp);

                // Set the previous value back. Need to a better way to handle this
                existingCamp.CampId = existingCampId;
                existingCamp.Location.LocationId = existingLocationId;

                if (await _campRepository.SaveChangesAsync())
                {
                    return _mapper.Map<CampModel>(existingCamp);
                }
            }
            catch (Exception)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Database Failure");
            }
            return BadRequest();
        }

        [HttpDelete("{moniker}")]
        public async Task<IActionResult> Delete(string moniker)
        {
            try
            {
                Camp existingCamp = await _campRepository.GetCampAsync(moniker);
                if (existingCamp == null) return NotFound();

                _campRepository.Delete(existingCamp);

                if (await _campRepository.SaveChangesAsync())
                {
                    return Ok();
                }
            }
            catch (Exception)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Database Failure");
            }
            return BadRequest("Failed to delete the camp");
        }

    }
}