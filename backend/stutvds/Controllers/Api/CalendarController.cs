using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StopStatAuth_6_0.Entities.Enums;
using stutvds.Controllers.Base;
using stutvds.DAL.Repositories;

namespace stutvds.Controllers
{
    [ApiController]
    [Route("api/calendar")]
    public class CalendarApiController : BaseController
    {
        private readonly DayLessonRepository _dayLessonRepository;

        public CalendarApiController(DayLessonRepository dayLessonRepository)
        {
            _dayLessonRepository = dayLessonRepository;
        }
        
        [HttpGet]
        [Authorize(Roles = "Admin, User")]
        public async Task<ActionResult<CalendarData>> GetCalendar(int year, int month)
        {
            var lessons = await _dayLessonRepository.GetAllByUserIdAndMonth(UserId, year, month + 1);

            var days = new List<DayData>();

            foreach (var lesson in lessons)
            {
                days.Add(new DayData
                {
                    LessonId = lesson.Id,
                    Date = lesson.StartTime.ToString("yyyy-MM-dd"),
                    Done = lesson.Status == LessonStatus.Finished,
                    Rewarded = lesson.Rewarded,
                    WordsRead = lesson.WordsSpoken
                });
            }

            var calendarDate = new CalendarData
            {
                Year = year,
                Month = month,
                Days = days
            };

            return Ok(calendarDate);
        }
    }
}