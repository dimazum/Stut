using System;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StopStatAuth_6_0.Entities.Enums;
using stutvds.Controllers.Base;
using stutvds.DAL.Entities;
using stutvds.DAL.Repositories;
using stutvds.Models.ClientDto;

namespace stutvds.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Admin, User")]
    public class LessonController: BaseController
    {
        private readonly DayLessonRepository _dayLessonRepository;
        private readonly IMapper _mapper;

        public LessonController(DayLessonRepository dayLessonRepository, IMapper mapper)
        {
            _dayLessonRepository = dayLessonRepository;
            _mapper = mapper;
        }
        
        [HttpGet("daily")]
        public async Task<ActionResult<DailyLessonDto>> GetDailyLesson()
        {
            var lesson = await _dayLessonRepository.GetByUserIdAndDay(UserId, DateTimeOffset.Now);
            
            if (lesson != null)
            {
                var mappedInt = _mapper.Map<DailyLessonDto>(lesson);
                return Ok(mappedInt);
            }

            var now = DateTimeOffset.Now;

            var newLesson = new DayLesson()
            {
                StartTime = now,
                StartRangeTime = now,
                LeftInSec = (int)TimeSpan.FromMinutes(15).TotalSeconds,
                Status = LessonStatus.None,
                UserId = UserId
            };
            
            var created = await _dayLessonRepository.AddAsync(newLesson);
            var mapped = _mapper.Map<DailyLessonDto>(created);
            
            return Ok(mapped);
        }

        [HttpGet("rewardPoints")]
        public async Task<ActionResult<int>> GetRewardPoints()
        {
            var points = await _dayLessonRepository.GetNotRewardedPoints(UserId);
            
            return Ok(points);
        }
        
        [HttpPut("resetRewardPoints")]
        public async Task<ActionResult<int>> ResetRewardPoints()
        {
            await _dayLessonRepository.ResetUserRewardedPoints(UserId);
            
            return Ok(0);
        }

        [HttpPost("start")]
        public async Task<IActionResult> StartLesson()
        {
            var lesson = await _dayLessonRepository.GetByUserIdAndDay(UserId, DateTimeOffset.Now);

            if (lesson == null)
            {
                throw new Exception("Lesson does not exist");
            }
            
            lesson.Status = LessonStatus.Started;
            lesson.StartTime = DateTimeOffset.Now;
            lesson.StartRangeTime = DateTimeOffset.Now;
            
            await _dayLessonRepository.UpdateAsync(lesson);

            return Ok(lesson);
        }
        
        [HttpPut("pause")]
        public async Task<IActionResult> PauseLesson([FromBody] DayLessonRequest request)
        {
            var lesson = await _dayLessonRepository.GetByIdAsync(request.Id);
            lesson.Status =  LessonStatus.Paused;

            await _dayLessonRepository.UpdateAsync(lesson);

            return Ok(lesson);
        }

        [HttpPut("finish")]
        public async Task<IActionResult> FinishLesson([FromBody] DayLessonRequest request)
        {
            var lesson = await _dayLessonRepository.GetByIdAsync(request.Id);
            
            lesson.Status = LessonStatus.Finished;
            lesson.LeftInSec = 0;
            lesson.FinishTime = DateTimeOffset.Now;
            lesson.RewardPoints += 7;
            
            await _dayLessonRepository.UpdateAsync(lesson);

            return Ok(lesson);
        }
        
        [HttpPut("reward")]
        public async Task<IActionResult> AddReward(RewardLessonRequest request)
        {
            if (request.Value == false)
            {
                return Ok();
            }
            
            var lesson = await _dayLessonRepository.GetByIdAsync(request.Id);

            lesson.Rewarded = request.Value;
            
            await _dayLessonRepository.UpdateAsync(lesson);

            return Ok();
        }
    }
}