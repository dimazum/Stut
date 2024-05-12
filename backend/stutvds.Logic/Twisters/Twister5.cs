using System.Collections.Generic;
using stutvds.Logic.Common;
using stutvds.Logic.Enums;

namespace stutvds.Logic.Twisters
{
    public class Twister5 : ITwister
    {
        public string Template => "сне-спе-сре ";

        public IEnumerable<string> GetTwister(int rows = 100, int columns = 4)
        {
            rows = 100;
            columns = 3;
            var consonantGenerator = new LetterGenerator(LetterVariant.Consonants);
            var vowelGenerator = new LetterGenerator(LetterVariant.AllVowels);

            for (int i = 0; i < rows; i++)
            {
                var v0 = vowelGenerator.GetRandomUnique();
                var c0 = consonantGenerator.GetRandomUnique();
                var c1 = consonantGenerator.GetRandomUnique();
                var c3 = consonantGenerator.GetRandomUnique();

                var word = string.Empty;
                for (int j = 0; j < columns; j++)
                {
                    word += $"с{c0}{v0} - с{c1}{v0} - c{c3}{v0}, ";
                }

                yield return word;
            }
        }
    }
}