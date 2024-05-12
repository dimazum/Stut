using System.Collections.Generic;
using stutvds.Logic.Common;
using stutvds.Logic.Enums;

namespace stutvds.Logic.Twisters
{
    public class Twister1 : ITwister
    {
        public string Template => "ааУму ааУму ааУму ааУму";

        public IEnumerable<string> GetTwister(int rows = 40, int columns = 4)
        {
            var consonantGenerator = new LetterGenerator(LetterVariant.Consonants);
            var vowelGenerator = new LetterGenerator(LetterVariant.AllVowels);

            for (int i = 0; i < rows; i++)
            {
                var v0 = vowelGenerator.GetRandomUnique();
                var v1 = vowelGenerator.GetRandomUnique();
                var c0 = consonantGenerator.GetRandomUnique();

                var word = string.Empty;
                for (int j = 0; j < columns; j++)
                {
                    word += $"{v0}{v0}{v1.ToUpper()}{c0}{v1}, ";
                }

                yield return word.TrimEnd();
            }
        }
    }
}