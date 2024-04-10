using System;
using StopStatAuth_6_0.Entities.Base;
using StopStatAuth_6_0.Entities.Enums;

namespace StopStatAuth_6_0.Entities
{
    public class TriggerEntity : Entity
    {
        public Guid? UserId { get; set; }
        public string Value { get; set; }
        public Language Language { get; set; }
        public bool? IsDefault { get; set; }
        public TriggerType TriggerType {get; set;}
        public DateTime? CreatedAt { get; set; }
        public int? Difficulty { get; set; }
    }
}
