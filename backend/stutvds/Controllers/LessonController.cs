using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using StopStatAuth_6_0.Entities.Enums;
using stutvds.Controllers.Base;
using stutvds.DAL.Entities;
using stutvds.DAL.Repositories;
using stutvds.Models.ClientDto;

namespace stutvds.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LessonController: BaseController
    {
        private readonly DayLessonRepository _dayLessonRepository;

        public LessonController(DayLessonRepository dayLessonRepository)
        {
            _dayLessonRepository = dayLessonRepository;
        }
        
        [HttpGet("daily")]
        public async Task<IActionResult> GetDailyLesson()
        {
            var lesson = await _dayLessonRepository.GetByUserIdAndDay(UserId, DateTimeOffset.Now);
            
            if (lesson != null)
            {
                return Ok(lesson);
            }

            var now = DateTimeOffset.Now;

            var newLesson = new DayLesson()
            {
                StartTime = now,
                StartRangeTime = now,
                LeftInSec = (int)TimeSpan.FromMinutes(15).TotalSeconds,
                Status = LessonStatus.Started,
                UserId = UserId
            };

            var created = await _dayLessonRepository.AddAsync(newLesson);
            
            return Ok(created);
        }
        
        [HttpPost("start")]
        public async Task<IActionResult> StartLesson()
        {
            var lesson = await _dayLessonRepository.GetByUserIdAndDay(UserId, DateTimeOffset.Now);

            if (lesson == null)
            {
                throw new Exception("Lesson does not exist");
            }
            
            if (lesson.Status == LessonStatus.Paused)
            {
                lesson.Status = LessonStatus.Started;
            }
            
            lesson.StartRangeTime = DateTimeOffset.Now;
            
            await _dayLessonRepository.UpdateAsync(lesson);

            return Ok(lesson);
        }
        
        [HttpPut("pause")]
        public async Task<IActionResult> PauseLesson([FromBody] DayLessonRequest request)
        {
            var lesson = await _dayLessonRepository.GetByIdAsync(request.Id);

            if (lesson.Status != LessonStatus.Finished)
            {
                var readSeconds = (int)(DateTimeOffset.Now - lesson.StartRangeTime).TotalSeconds;
                lesson.LeftInSec = Math.Max(0, lesson.LeftInSec - readSeconds);
            }
            
            lesson.Status = LessonStatus.Paused;
            lesson.WordsSpoken = request.Words;
            lesson.WPS = request.Wps;
            await _dayLessonRepository.UpdateAsync(lesson);

            return Ok(lesson);
        }

        [HttpPut("finish")]
        public async Task<IActionResult> FinishLesson([FromBody] DayLessonRequest request)
        {
            var lesson = await _dayLessonRepository.GetByIdAsync(request.Id);
            
            lesson.WordsSpoken = request.Words;
            lesson.WPS = request.Wps;
            lesson.Status = LessonStatus.Finished;
            lesson.LeftInSec = 0;
            lesson.FinishTime = DateTimeOffset.Now;

            var dates = await _dayLessonRepository.GetLastLessonDates(UserId, 2);

            var twoDayStreak =
                dates.Contains(DateTimeOffset.UtcNow.Date.AddDays(-1)) &&
                dates.Contains(DateTimeOffset.UtcNow.Date.AddDays(-2));

            var oneDayStreak =
                dates.Contains(DateTimeOffset.UtcNow.Date.AddDays(-1));

            var rewardPoints = 0;

            if (twoDayStreak)
            {
                rewardPoints = 14;
            }
            else if (oneDayStreak)
            {
                rewardPoints = 12;
            }
            else
            {
                rewardPoints = 10;
            }
            
            lesson.RewardPoints += rewardPoints;
            
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