using AutoMapper;
using CoreCodeCamp.Data;
using CoreCodeCamp.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using System;
using System.Threading.Tasks;

namespace CoreCodeCamp.Controllers
{
    [ApiController]
    [Route("api/camps/{moniker}/talks")]
    public class TalksController : ControllerBase
    {
        private readonly ICampRepository _campRepository;
        private readonly IMapper _mapper;
        private readonly LinkGenerator _linkGenerator;

        public TalksController(ICampRepository campRepository, IMapper mapper, LinkGenerator linkGenerator)
        {
            _campRepository = campRepository;
            _mapper = mapper;
            _linkGenerator = linkGenerator;
        }

        [HttpGet]
        public async Task<ActionResult<TalkModel[]>> Get(string moniker)
        {
            try
            {
                Talk[] talks = await _campRepository.GetTalksByMonikerAsync(moniker, true);

                return _mapper.Map<TalkModel[]>(talks);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Failed to get Talks");

            }

        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<TalkModel>> Get(string moniker, int id)
        {
            try
            {
                var talk = await _campRepository.GetTalkByMonikerAsync(moniker, id, true);
                return _mapper.Map<TalkModel>(talk);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Failed to get that talk");
            }
        }

        [HttpPost]
        public async Task<ActionResult<TalkModel>> Post(string moniker, TalkModel talkModel)
        {
            try
            {
                Camp camp = await _campRepository.GetCampAsync(moniker);
                if (camp == null) return BadRequest("Camp does not exist");

                Talk talk = _mapper.Map<Talk>(talkModel);
                talk.Camp = camp;

                // Check to see if there is a speaker in the talkModel.
                if (talkModel.Speaker == null) return BadRequest("Speaker ID is required");
                Speaker speaker = await _campRepository.GetSpeakerAsync(talkModel.Speaker.SpeakerId);
                if (speaker == null) return BadRequest("Speaker could not be found");

                talk.Speaker = speaker;
                _campRepository.Add(talk);

                if (await _campRepository.SaveChangesAsync())
                {
                    string url = _linkGenerator.GetPathByAction(HttpContext,
                                 "Get", values: new { moniker, id = talk.TalkId });
                    return Created(url, _mapper.Map<TalkModel>(talk));
                }
                else
                {
                    return BadRequest("Faled to save new Talk");                 
                }
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Failed to get that talk");
            }

        }
    }
}