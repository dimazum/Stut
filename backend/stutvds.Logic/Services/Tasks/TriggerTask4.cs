using System;
using System.Collections.Generic;
using stutvds.Logic.Common;
using stutvds.Logic.DTOs;
using stutvds.Logic.Enums;

namespace stutvds.Logic.Services.Tasks
{
    public class TriggerTask4 : ITriggerTask
    {
        public string Name => "101 - кролик, 102 - кролик, 103 - кролик, 104 - кролик,";
        public TriggerTaskType Type => TriggerTaskType.Type4;
        public bool IsStretch => false;

        public TriggerTaskResult GetTask(string triggerValue, int rows = 20, int columns = 4)
        {
            var triggerValues = new List<string>();

            var random = new Random();

            var val = random.Next(100, 900);

            for (int i = 0; i < rows; i++)
            {
                var word = string.Empty;
    
                word += $"{val++} {triggerValue}, {val++} {triggerValue}, {val++} {triggerValue}, {val++} {triggerValue}";

                triggerValues.Add(word);
            }

            return new TriggerTaskResult()
            {
                Values = triggerValues,
                Description = $"Прочитайте эти {rows} строк",
                IsStretch = IsStretch
            };
        }
    }
}