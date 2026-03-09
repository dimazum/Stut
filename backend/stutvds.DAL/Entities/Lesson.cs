using System;
using System.ComponentModel.DataAnnotations.Schema;
using StopStatAuth_6_0.Entities.Base;
using StopStatAuth_6_0.Entities.Enums;

namespace stutvds.DAL.Entities
{
    public class DayLesson: Entity
    {
        public Guid UserId { get; set; }
        public LessonStatus Status { get; set; }
        public bool Rewarded { get; set; }
        /// <summary>
        /// Начало урока
        /// </summary>
        public DateTimeOffset StartTime { get; set; }
        /// <summary>
        /// Время нажатия кнопки "Cтарт". Нужно для расчета пауз.
        /// </summary>
        public DateTimeOffset StartRangeTime { get; set; }
        /// <summary>
        /// Время финиша урока
        /// </summary>
        public DateTimeOffset? FinishTime { get; set; }
        public int LeftInSec { get; set; }
        public int WordsSpoken { get; set; }
        public int WPS { get; set; } //words per second
        public int RewardPoints { get; set; }
    }
}