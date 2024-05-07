using System.Collections.Generic;
using stutvds.Logic.Enums;

namespace stutvds.Logic.Twisters
{
    public class Twister3 : ITwister
    {
        public string Template => "мнис, мнис, мнис, ";

        public IEnumerable<string> GetTwister(int rows = 40, int columns = 4)
        {
            var consonantGenerator = new LetterGenerator(LetterVariant.Consonants);

            for (int i = 0; i < rows; i++)
            {
                var word = string.Empty;
                for (int j = 0; j < columns; j++)
                {
                    var c0 = consonantGenerator.GetRandomUnique();

                    word += $"{c0}ай, ";
                }

                yield return word;
            }
        }
    }
}