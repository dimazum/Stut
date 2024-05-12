using System.Collections.Generic;
using stutvds.Logic.Common;
using stutvds.Logic.DTOs;
using stutvds.Logic.Enums;

namespace stutvds.Logic.Services.Tasks
{
    public class TriggerTask1 : ITriggerTask
    {
        private readonly SyllablesBreaker _syllablesBreaker;
        public string Name => "Растягиваем мышцы языка";
        public TriggerTaskType Type => TriggerTaskType.Type1;
        public bool IsStretch => true;

        public TriggerTask1(SyllablesBreaker syllablesBreaker)
        {
            _syllablesBreaker = syllablesBreaker;
        }

        public TriggerTaskResult GetTask(string triggerValue, int rows = 40, int columns = 4)
        {
            var syllables = _syllablesBreaker.GetSyllables(triggerValue);
            
            var triggerValues = new List<string>();

            for (int i = 0; i < syllables.Count; i++)
            {
                string words = "";
                for (int j = 0; j < columns; j++)
                {
                    words += $"{syllables[i]}";
                }
                
                triggerValues.Add($"{words}");
                triggerValues.Add($"{words}");
                triggerValues.Add($"{words}");
                triggerValues.Add($"{words}");
                triggerValues.Add($"{words}");
            }
            
            return new TriggerTaskResult()
            {
                Values = triggerValues,
                Description = $"Прочитайте эти строки, тянув корень языка в разные строны указанные стрелками",
                IsStretch = IsStretch
            };
        }
    }
}