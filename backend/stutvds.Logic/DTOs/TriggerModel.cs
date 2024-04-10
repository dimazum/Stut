using System;
using StopStatAuth_6_0.Entities.Enums;

namespace stutvds.Logic.DTOs
{
    public class TriggerModel
    {
        public Guid? UserId;
        public string Trigger { get; set; }
        public Language Language { get; set; }
        public bool? IsDefault { get; set; }
        public TriggerType TriggerType { get; set; }
        public DateTime? CreatedAt { get; set; }
        public int? Difficulty { get; set; }
    }
}