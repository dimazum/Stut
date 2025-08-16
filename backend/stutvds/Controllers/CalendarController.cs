using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;

namespace stutvds.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CalendarController : ControllerBase
    {
        // GET: api/calendar/get
        [HttpGet("get")]
        public ActionResult<CalendarData> GetCalendar()
        {
            var today = DateTime.Today;
            int year = today.Year;
            int month = today.Month - 1; // JS/Angular ожидает 0-based месяц

            var days = new List<DayData>();

            // Пример: отметим несколько случайных дней как выполненные
            for (int day = 1; day <= DateTime.DaysInMonth(year, month + 1); day++)
            {
                var date = new DateTime(year, month + 1, day);
                days.Add(new DayData
                {
                    Date = date.ToString("yyyy-MM-dd"),
                    Done = day % 3 == 0, // пример: каждый 3-й день выполнен
                    WordsRead = day * 10  // просто пример числа слов
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