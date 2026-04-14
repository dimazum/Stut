using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using StopStatAuth_6_0.Entities.Enums;
using stutvds.Controllers.Base;
using stutvds.DAL;
using stutvds.DAL.Entities;
using stutvds.Models.ClientDto;

namespace stutvds.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TriggerController : BaseController
    {
        private readonly TriggerRepository _triggerRepository;
        private readonly IMapper _mapper;
        private readonly TriggerGeneratorService _triggerService;

        public TriggerController(
            TriggerRepository triggerRepository,
            IMapper mapper,
            TriggerGeneratorService triggerService)
        {
            ILogger logger;
            _triggerRepository = triggerRepository;
            _mapper = mapper;
            _triggerService = triggerService;
        }

        [HttpPost]
        [Route("create")]
        public async Task<IActionResult> Create([FromBody] TriggerClientDto dto)
        {
            var lettersCount = dto.Value.Length;

            if (lettersCount > 60)
            {
                return BadRequest(new
                {
                    code = ErrorCodes.ValidationError.Code,
                    message = $"Trigger :'{dto.Value}' too long (60 character long max)."
                });
            }


            var userId = Guid.Parse(GetUserId());
            var isExisted = await _triggerRepository.IfExistsAsync(x => 
                x.Value == dto.Value && x.UserId == userId);

            if (isExisted)
            {
                return NotFound(new
                {
                    code = ErrorCodes.NotFound.Code,
                    message = ErrorCodes.NotFound.Message,
                });
            }

            var entity = new TriggerEntity()
            {
                Value = dto.Value,
                UserId = UserId,
                IsDefault = false,
                CreatedAt = DateTime.Now,
                Difficulty = dto.Difficulty,
                Language = Language.Russian
            };

            await _triggerRepository.AddAsync(entity);

            var triggers = _triggerRepository.GetTriggers(UserId, CurrentLanguage);

            return new JsonResult(triggers
                .Select(t => new
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
            var entity = await _triggerRepository.GetByName(triggerValue, UserId);

            await _triggerRepository.DeleteAsync(entity);

            var triggers = _triggerRepository.GetTriggers(UserId, CurrentLanguage);

            return new JsonResult(triggers
                .Select(t => new
                {
                    t.Value,
                    CreatedAt = t.CreatedAt,
                    t.Difficulty
                }));
        }

        [HttpGet]
        public JsonResult Get()
        {
            var triggers = _triggerRepository.GetTriggers(UserId, CurrentLanguage);

            var mapped = _mapper.Map<List<TriggerResultClientDto>>(triggers);

            return new JsonResult(mapped);
        }
        
        [HttpGet]
        [Route("last")]
        public JsonResult GetLast()
        {
            var triggers = _triggerRepository.GetLastTriggers(UserId, 15, CurrentLanguage);

            var mapped = _mapper.Map<List<TriggerResultClientDto>>(triggers);

            return new JsonResult(mapped);
        }

        [HttpPut]
        [Route("changedifficulty")]
        public async Task<JsonResult> ChangeDifficulty(TriggerChangeDifficultyDto dto)
        {
            var entity = await _triggerRepository.GetByName(dto.TriggerValue, UserId);
            entity.Difficulty = dto.Difficulty;

            await _triggerRepository.UpdateAsync(entity);

            var triggers = _triggerRepository.GetTriggers(UserId, CurrentLanguage);

            return new JsonResult(triggers.Select(t => new
            {
                t.Value,
                CreatedAt = t.CreatedAt,
                t.Difficulty
            }));
        }

        [HttpGet("seed")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> SeedTriggers()
        {
            try
            {
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "data", "ru", "triggers.txt");

                if (!System.IO.File.Exists(filePath))
                    return NotFound("Файл triggers.txt не найден.");

                var lines = await System.IO.File.ReadAllLinesAsync(filePath);

                var words = lines
                    .Where(l => !string.IsNullOrWhiteSpace(l))
                    .Select(l => l.Trim())
                    .Distinct()
                    .ToList();

                var now = DateTime.UtcNow;

                var triggers = words.Select(word => new TriggerEntity
                {
                    UserId = null,
                    Value = word,
                    Language = Language.Russian,
                    IsDefault = true,
                    TriggerType = TriggerType.Short,
                    CreatedAt = now,
                    Difficulty = 0
                }).ToList();

                await _triggerRepository.SeedDefaultTriggers(triggers);

                return Ok(new { Count = triggers.Count, Message = "Слова успешно засеяны." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Ошибка при посеве: {ex.Message}");
            }
        }
    }
}