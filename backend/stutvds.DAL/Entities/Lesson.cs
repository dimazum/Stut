using System;
using StopStatAuth_6_0.Entities.Base;
using StopStatAuth_6_0.Entities.Enums;

namespace stutvds.DAL.Entities
{
    public class DayLesson: Entity
    {
        public Guid UserId { get; set; }
        public LessonStatus Status { get; set; }
        public DateTime Date { get; set; }
        public int WordsSpoken { get; set; }
        public int WPS { get; set; } //words per second
    }
}