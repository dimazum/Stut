using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using stutvds.Controllers.Base;
using stutvds.Logic.DTOs;
using stutvds.Logic.Services.Contracts;
using stutvds.Logic.Services.Tasks;
using stutvds.Models.ClientDto;

namespace stutvds.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TriggerController : BaseController
    {
        private readonly ITriggerService _triggerService;
        private readonly IMapper _mapper;
        private readonly TriggerTaskManager _triggerTaskManager;

        public TriggerController(
            ITriggerService triggerService,
            IMapper mapper,
            TriggerTaskManager triggerTaskManager )
        {
            ILogger logger;
            _triggerService = triggerService;
            _mapper = mapper;
            _triggerTaskManager = triggerTaskManager;
        }

        [HttpPost]
        [Route("create")]
        public async Task<JsonResult> Create([FromBody]TriggerClientDto dto)
        {
            var mapped = _mapper.Map<TriggerModel>(dto);
            mapped.Language = CurrentLanguage;

            await _triggerService.CreateAsync(mapped);

            var triggers = _triggerService.GetTriggers(CurrentLanguage, UserId);

            return new JsonResult(triggers.Select(t => new
            {
                t.Value,
                t.CreatedAt,
                t.Difficulty
            }));
        }

        [HttpDelete]
        [Route("{triggerValue}")]
        public async Task<JsonResult> Delete(string triggerValue)
        {
            await _triggerService.DeleteAsync(triggerValue);

            var triggers = _triggerService.GetTriggers(CurrentLanguage, UserId);

            return new JsonResult(triggers.Select(t => new
            {
                t.Value,
                CreatedAt = t.CreatedAt,
                t.Difficulty
            }));
        }

        [HttpGet]
        public JsonResult Get()
        {
            var triggers = _triggerService.GetTriggers(CurrentLanguage, UserId);

            var mapped = _mapper.Map<List<TriggerResultClientDto>>(triggers);

            return new JsonResult(mapped);
        }

        [HttpPut]
        [Route("change")]
        public async Task<JsonResult> ChangeDifficulty(string trigger, int difficulty)
        {
            var triggerModel = new TriggerModel()
            {
                Value = trigger,
                Difficulty = difficulty
            };

            await _triggerService.UpdateTriggerAsync(triggerModel);

            var triggers = _triggerService.GetTriggers(CurrentLanguage, UserId);

            return new JsonResult(triggers.Select(t => new
            {
                t.Value,
                CreatedAt = t.CreatedAt,
                t.Difficulty
            }));
        }
        
        [HttpGet]
        [Route("triggertasks/{triggerValue}")]
        public JsonResult GetTriggerTasks(string triggerValue)
        {
            var tasks = _triggerTaskManager.GetTriggerTasks(triggerValue);
            
            //кро - кро- кро - кро(5 раз)
            //лик - лик - лик - лик (5 раз)
            //кролик - ааз - икук
            //вде - кролик - ист
            //сде - кде - иыт - кролик
            // 101 кролик - 102 кролик - 1

            return new JsonResult(tasks);
        } 
    }
}