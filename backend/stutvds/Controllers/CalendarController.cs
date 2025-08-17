using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using StopStatAuth_6_0.Entities.Enums;
using stutvds.Controllers.Base;
using stutvds.DAL.Repositories;

namespace stutvds.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CalendarController : BaseController
    {
        private readonly DayLessonRepository _dayLessonRepository;

        public CalendarController(DayLessonRepository dayLessonRepository)
        {
            _dayLessonRepository = dayLessonRepository;
        }
        
        [HttpGet("get")]
        public async Task<ActionResult<CalendarData>> GetCalendar()
        {
            var today = DateTime.Today;
            int year = today.Year;
            int month = today.Month - 1; // JS/Angular ожидает 0-based месяц
            
            if (!UserId.HasValue)
            {
                throw new Exception("User is not logged in");
            }

            var lessons = await _dayLessonRepository.GetAllByUserIdAndMonth(UserId.Value, today);

            var days = new List<DayData>();

            foreach (var lesson in lessons)
            {
                days.Add(new DayData
                {
                    Date = lesson.Date.ToString("yyyy-MM-dd"),
                    Done = lesson.Status == LessonStatus.Finished,
                    WordsRead = lesson.WordsSpoken
                });
            }

            var calendar = new CalendarData
            {
                Year = year,
                Month = month,
                Days = days
            };

            return Ok(calendar);
        }
    }
}