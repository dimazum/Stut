using System;
using System.Collections.Generic;
using System.Linq;
using stutvds.Logic.Enums;

namespace stutvds.Logic.Common
{
    public class LetterGenerator
    {
        private readonly List<string>  _allVowels = new List<string> { "а", "о", "у", "э", "ы", "и", "я", "ю", "е", "ё" };
        private readonly List<string> _vowels1 = new List<string> { "а", "о", "у", "э", "ы", "и"};
        private readonly List<string> _consonants = new List<string> { "п", "б", "ф", "в", "к", "г", "т", "д", "ш", "ж", "с", "з", "л", "м", "н", "р", "х", "ц", "ч" };
        private readonly LetterVariant _letterVariant;

        Stack<string> _shaffledStrings;

        public LetterGenerator(LetterVariant letterVariant)
        {
            _letterVariant = letterVariant;
        }

        public string GetRandomUnique()
        {
            if (_shaffledStrings == null || _shaffledStrings.Count == 0)
            {
                switch (_letterVariant)
                {
                    case LetterVariant.None:
                        _shaffledStrings = new Stack<string>();
                        break;
                    case LetterVariant.AllVowels:
                        _shaffledStrings = ShuffleIntList(_allVowels.ToList());
                        break;
                    case LetterVariant.Vowels1:
                        _shaffledStrings = ShuffleIntList(_vowels1.ToList());
                        break;
                    case LetterVariant.Consonants:
                        _shaffledStrings = ShuffleIntList(_consonants.ToList());
                        break;
                    default:
                        _shaffledStrings = new Stack<string>();
                        break;
                }
            }

            return _shaffledStrings.Pop();
        }

        private static Stack<string> ShuffleIntList(List<string> list)
        {
            var random = new Random();
            var newShuffledList = new List<string>();
            var listcCount = list.Count;
            for (int i = 0; i < listcCount; i++)
            {
                var randomElementInList = random.Next(0, list.Count);
                newShuffledList.Add(list[randomElementInList]);
                list.Remove(list[randomElementInList]);
            }

            var shafledStrings = new Stack<string>(newShuffledList);

            return shafledStrings;
        }
    }
}
