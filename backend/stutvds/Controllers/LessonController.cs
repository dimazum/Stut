using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using StopStatAuth_6_0.Entities.Enums;
using stutvds.Controllers.Base;
using stutvds.DAL.Entities;
using stutvds.DAL.Repositories;

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
        
        [HttpPost("start")]
        public async Task<IActionResult> StartLesson()
        {
            if (!UserId.HasValue)
            {
                throw new Exception("User is not logged in");
            }

            var lesson = await _dayLessonRepository.GetByUserIdAndDay(UserId.Value, DateTime.UtcNow);

            if (lesson != null)
            {
                return Ok(lesson.Id);
            }

            var newLesson = new DayLesson()
            {
                Date = DateTime.UtcNow,
                Status = LessonStatus.Started,
                UserId = UserId.Value
            };

            var created = await _dayLessonRepository.AddAsync(newLesson);
            
            
            return Ok(created.Id);
        }

        [HttpPut("finish")]
        public async Task<IActionResult> FinishLesson(int id, int words, int wps)
        {
            var lesson = await _dayLessonRepository.GetByIdAsync(id);

            lesson.WordsSpoken = words;
            lesson.WPS = wps;
            lesson.Status = LessonStatus.Finished;
            
            await _dayLessonRepository.UpdateAsync(lesson);

            return Ok();
        }
        
        [HttpPut("reward")]
        public async Task<IActionResult> AddReward(int id)
        {
            var lesson = await _dayLessonRepository.GetByIdAsync(id);

            lesson.Status = LessonStatus.Rewarded;
            
            await _dayLessonRepository.UpdateAsync(lesson);

            return Ok();
        }
    }
}