using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using StopStatAuth_6_0.Entities.Enums;
using stutvds.Constants;
using stutvds.Controllers.Base;
using stutvds.DAL;
using stutvds.DAL.Entities;
using stutvds.Logic.Services.Tasks;
using stutvds.Models.ClientDto;

namespace stutvds.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TriggerController : BaseController
    {
        private readonly TriggerRepository _triggerRepository;
        private readonly IMapper _mapper;
        private readonly TriggerTaskManager _triggerTaskManager;
        private readonly TriggerGeneratorService _triggerService;

        public TriggerController(
            TriggerRepository triggerRepository,
            IMapper mapper,
            TriggerTaskManager triggerTaskManager,
            TriggerGeneratorService triggerService)
        {
            ILogger logger;
            _triggerRepository = triggerRepository;
            _mapper = mapper;
            _triggerTaskManager = triggerTaskManager;
            _triggerService = triggerService;
        }

        [HttpPost]
        [Route("create")]
        public async Task<JsonResult> Create([FromBody]TriggerClientDto dto)
        {
            var isExisted = await _triggerRepository.IfExistsAsync(x => x.Value == dto.Value);

            if (isExisted)
            {
                throw new InvalidOperationException($"Trigger :'{dto.Value}' already exists.");
            }
            
            var entity = new TriggerEntity()
            {
                Value = dto.Value,
                UserId = UserId,
                IsDefault = true,
                CreatedAt = DateTime.Now,
                Difficulty = dto.Difficulty,
                Language = Language.Russian
            };

            var trigger=  await _triggerRepository.AddAsync(entity);

            var triggers = _triggerRepository.GetTriggers( UserId, CurrentLanguage);

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

            var triggers = _triggerRepository.GetTriggers( UserId, CurrentLanguage);

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

        [HttpPut]
        [Route("changedifficulty")]
        public async Task<JsonResult> ChangeDifficulty(TriggerChangeDifficultyDto dto)
        {
            var entity = await _triggerRepository.GetByName(dto.TriggerValue, UserId);
            entity.Difficulty = dto.Difficulty;

            await _triggerRepository.UpdateAsync(entity);
            
            var triggers =  _triggerRepository.GetTriggers(UserId, CurrentLanguage);

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
        
        [HttpGet]
        [Route("randomWord/{letter}/{count}")]
        public ActionResult GetRandomWord(string letter, int count)
        {
            string words = string.Empty;
            string w = String.Empty;
            
            var random = new Random();
            
            var l = letter.ToLowerInvariant();
            
            switch (l)
            {
                case "a":
                {
                    var all = Triggers.GetTriggers_A();
                    var r = random.Next(all.Count - 1);
                     w = all[r];
                } break;
                case "о":
                {
                    var all = Triggers.GetTriggers_A();
                    var r = random.Next(all.Count - 1);
                    w = all[r];
                } break;
                case "у":
                {
                    var all = Triggers.GetTriggers_A();
                    var r = random.Next(all.Count - 1);
                    w = all[r];
                } break;
                case "э":
                {
                    var all = Triggers.GetTriggers_A();
                    var r = random.Next(all.Count - 1);
                    w = all[r];
                } break;
            }
            
            var sb = new StringBuilder();

            for (int i = 0; i < count; i++)
            {
                sb.Append(w);
                sb.Append(", ");
            }
                   
            words = sb.ToString().Trim().Trim();

            return Ok(words);
        } 
        
        
        [HttpGet("generate")]
        public ActionResult<Dictionary<string, List<string>>> GenerateTriggers()
        {
            var triggerWords = _triggerRepository
                .GetLastTriggers(UserId, 3, CurrentLanguage)
                .Select(x => x.Value)
                .ToList();

            if (!triggerWords.Any())
            {
                triggerWords = Triggers.GetRandomThreeTriggers_P();
            }
            
            var result = new Dictionary<string, List<string>>();

            foreach (var word in triggerWords)
            {
                // Генерируем 10 триггеров
                var triggerExs = _triggerService.GenerateTriggers(word, 10);
                var formattedLines = new List<string>();

                foreach (var triggerWord in triggerExs)
                {
                    // Разделяем триггер и слово
                    int spaceIndex = triggerWord.IndexOf(' ');
                    string trigger = spaceIndex > 0 ? triggerWord.Substring(0, spaceIndex) : triggerWord;

                    // Четыре повтора через запятую
                    //string line = $"{trigger} {word}, но {word}";
                    string line = $"{word}, но {word}";
                    formattedLines.Add(line);
                }

                result[$"{word}"] = formattedLines;
            }

            return Ok(result);
        }
    }
}