using System.Collections.Generic;
using stutvds.Logic.Common;
using stutvds.Logic.DTOs;
using stutvds.Logic.Enums;

namespace stutvds.Logic.Services.Tasks
{
    public class TriggerTask4 : ITriggerTask
    {
        public string Name => "сде - кде - иыт - кролик";
        public TriggerTaskType Type => TriggerTaskType.Type4;
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
                var v2 = vowelGenerator.GetRandomUnique();
                var c0 = consonantGenerator.GetRandomUnique();
                var c1 = consonantGenerator.GetRandomUnique();
                var c2 = consonantGenerator.GetRandomUnique();
                var c3 = consonantGenerator.GetRandomUnique();

                var word = string.Empty;
                for (int j = 0; j < columns; j++)
                {
                    word += $"c{c1}{v0} - {c0}{c1}{v0} - {v1}{v2}{c3} - {triggerValue}, ";
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