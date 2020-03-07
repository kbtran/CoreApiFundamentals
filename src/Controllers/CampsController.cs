﻿using System.Threading.Tasks;
using AutoMapper;
using CoreCodeCamp.Data;
using CoreCodeCamp.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CoreCodeCamp.Controllers
{
    [Route("api/[controller]")]
    public class CampsController : ControllerBase
    {
        private readonly ICampRepository _campRepository;
        private readonly IMapper _mapper;
        public CampsController(ICampRepository campRepository, IMapper mapper)
        {
            _campRepository = campRepository;
            _mapper = mapper;
        }

        //[HttpGet]
        //public IActionResult Get()
        //{
        //   // if (false) return BadRequest("Bad stuff happens");
        //    return Ok(new { Moniker = "ATL2020", Name = "Atlanta Code Camp" });
        //}

        [HttpGet]
        public async Task<ActionResult<CampModel[]>> Get()
        {
            try
            {
                // get camps from database
                var camps = await _campRepository.GetAllCampsAsync();
           
                return _mapper.Map<CampModel[]>(camps);
            }
            catch
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Database Failure");
            }
            
        }
    }
}