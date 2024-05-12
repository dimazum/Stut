using System.Collections.Generic;
using stutvds.Logic.Common;
using stutvds.Logic.DTOs;
using stutvds.Logic.Enums;

namespace stutvds.Logic.Services.Tasks
{
    public class TriggerTask3 : ITriggerTask
    {
        public string Name => "вде - кролик - ист";
        public TriggerTaskType Type => TriggerTaskType.Type3;
        public bool IsStretch => false;

        public TriggerTaskResult GetTask(string triggerValue, int rows = 40, int columns = 4)
        {
            var consonantGenerator = new LetterGenerator(LetterVariant.Consonants);
            var vowelGenerator = new LetterGenerator(LetterVariant.AllVowels);

            var triggerValues = new List<string>();

            for (int i = 0; i < rows; i++)
            {
                var v0 = vowelGenerator.GetRandomUnique();
                var v1 = vowelGenerator.GetRandomUnique();
                var c0 = consonantGenerator.GetRandomUnique();
                var c1 = consonantGenerator.GetRandomUnique();
                var c2 = consonantGenerator.GetRandomUnique();
                var c3 = consonantGenerator.GetRandomUnique();

                var word = string.Empty;
                for (int j = 0; j < columns; j++)
                {
                    word += $"{c0}{c1}{v0} - {triggerValue} - {v1}{c2}{c3}, ";
                }

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