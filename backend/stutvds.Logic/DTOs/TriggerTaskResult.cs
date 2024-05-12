using System.Collections.Generic;

namespace stutvds.Logic.DTOs
{
    public class TriggerTaskResult
    {
        public string Description { get; set; }
        public bool IsStretch { get; set; }
        public List<string> Values { get; set; }
    }
}