using System.Collections.Generic;
using stutvds.Logic.DTOs;
using stutvds.Logic.Enums;

namespace stutvds.Logic.Services.Tasks
{
    public interface ITriggerTask
    {
        public string Name { get; }
        public TriggerTaskType Type { get; }
        public bool IsStretch { get; }
        TriggerTaskResult GetTask(string triggerValue, int rows = 40, int columns = 4);
    }
}