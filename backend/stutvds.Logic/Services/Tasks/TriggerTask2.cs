using System.Collections.Generic;
using stutvds.Logic.Common;
using stutvds.Logic.DTOs;
using stutvds.Logic.Enums;

namespace stutvds.Logic.Services.Tasks
{
    public class TriggerTask2 : ITriggerTask
    {
        public string Name => "ээц - апип - кролик, ээц - апип - кролик, ээц - апип - кролик";
        public TriggerTaskType Type => TriggerTaskType.Type2;
        public bool IsStretch => false;

        public TriggerTaskResult GetTask(string triggerValue, int rows = 40, int columns = 4)
        {
            var consonantGenerator = new LetterGenerator(LetterVariant.Consonants);
            var vowelGenerator = new LetterGenerator(LetterVariant.AllVowels);
            var vowelGenerator1 = new LetterGenerator(LetterVariant.Vowels1);

            var triggerValues = new List<string>();

            for (int i = 0; i < rows; i++)
            {
                var v0 = vowelGenerator1.GetRandomUnique();
                var v1 = vowelGenerator1.GetRandomUnique();
                var v2 = vowelGenerator.GetRandomUnique();
                var c0 = consonantGenerator.GetRandomUnique();
                var c1 = consonantGenerator.GetRandomUnique();

                var word = string.Empty;
                for (int j = 0; j < columns; j++)
                {
                    word += $"{v0}{v0}{c0} - {v1}{c1}{v2}{c1} - {triggerValue}, ";
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