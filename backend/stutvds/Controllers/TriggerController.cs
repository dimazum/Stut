using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using StopStatAuth_6_0.Entities.Enums;
using stutvds.Controllers.Base;
using stutvds.DAL.Entities;
using stutvds.Logic.DTOs;
using stutvds.Logic.Services.Contracts;
using stutvds.Models.ClientDto;

namespace stutvds.Controllers
{
    public class TriggerController : BaseController
    {
        private readonly ITriggerService _triggerService;

        public IEnumerable<TriggerEntity> Triggers;

        public TriggerController(ITriggerService triggerService)
        {
            ILogger logger;
            _triggerService = triggerService;
        }

        [HttpPost]
        public async Task<JsonResult> Create(string trigger, int difficulty)
        {
            var triggerModel = new TriggerModel()
            {
                Trigger = trigger,
                Difficulty = difficulty
            };

            await _triggerService.CreateAsync(triggerModel);

            var triggers = _triggerService.GetTriggers(CurrentLanguage, UserId);

            return new JsonResult(triggers.Select(t => new
            {
                t.Trigger,
                t.CreatedAt,
                t.Difficulty
            }));
        }

        [HttpPost]
        public async Task<JsonResult> Delete(string trigger)
        {
            var triggerModel = new TriggerModel()
            {
                Trigger = trigger,
            };

            await _triggerService.DeleteAsync(triggerModel);

            var triggers = _triggerService.GetTriggers(CurrentLanguage, UserId);

            return new JsonResult(triggers.Select(t => new
            {
                t.Trigger,
                CreatedAt = t.CreatedAt,
                t.Difficulty
            }));
        }

        [HttpGet]
        public JsonResult Get()
        {
            var triggers = _triggerService.GetTriggers(CurrentLanguage, UserId);

            return new JsonResult(triggers.Select(t => new
            {
                t.Trigger,
                CreatedAt = t.CreatedAt,
                t.Difficulty
            }));
        }

        [HttpPost]
        public async Task<JsonResult> ChangeDifficulty(string trigger, int difficulty)
        {
            var triggerModel = new TriggerModel()
            {
                Trigger = trigger,
                Difficulty = difficulty
            };

            await _triggerService.UpdateTriggerAsync(triggerModel);

            var triggers = _triggerService.GetTriggers(CurrentLanguage, UserId);

            return new JsonResult(triggers.Select(t => new
            {
                t.Trigger,
                CreatedAt = t.CreatedAt,
                t.Difficulty
            }));
        }
    }
}