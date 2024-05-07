using System.Collections.Generic;
using stutvds.Logic.Enums;

namespace stutvds.Logic.Twisters
{
    public class Twister2 : ITwister
    {
        public string Template => "ээц - апип, ээц - апип, ээц - апип, ээц - апип";

        public IEnumerable<string> GetTwister(int rows = 40, int columns = 4)
        {
            var consonantGenerator = new LetterGenerator(LetterVariant.Consonants);
            var vowelGenerator = new LetterGenerator(LetterVariant.AllVowels);
            var vowelGenerator1 = new LetterGenerator(LetterVariant.Vowels1);

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
                    word += $"{v0}{v0}{c0} - {v1}{c1}{v2}{c1}, ";
                }

                yield return word;
            }
        }
    }
}