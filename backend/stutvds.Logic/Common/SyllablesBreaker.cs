using System.Collections.Generic;

namespace stutvds.Logic.Common
{
    public class SyllablesBreaker
    {
        public List<string> GetSyllables(string word)
        {
            var syllables = new List<string>();
            var syllable = new List<char>();

            for (int i = 0; i < word.Length; i++)
            {
                char currentChar = word[i];
                char nextChar = (i < word.Length - 1) ? word[i + 1] : '\0';
            
                bool isCurrentCharLast = i == word.Length - 1;
                bool isNextCharLast = i == word.Length - 2;
                
                syllable.Add(currentChar);

                if (!isCurrentCharLast && !isNextCharLast && IsVowel(currentChar) && !IsVowel(nextChar))
                {
                    string s = new string(syllable.ToArray());
                    syllables.Add(s);
                    syllable.Clear();
                }

                if (isCurrentCharLast)
                {
                    string s = new string(syllable.ToArray());
                    syllables.Add(s);
                }
            }

            return syllables;
        }
        
        private bool IsVowel(char c)
        {
            return "аоуыэяёюиеАОУЫЭЯЁЮИЕaeiouAEIOU".Contains(c);
        }
    }
}