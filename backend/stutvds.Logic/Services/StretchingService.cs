using System.Collections.Generic;
using stutvds.Logic.Services.Contracts;

namespace stutvds.Logic.Services
{
    public class StretchingService : IStretchingService
    {
        public IEnumerable<IEnumerable<string>> GetAll()
        {
            yield return GenerateStr0();
        }

        //ааа-ааа-ааа
        private IEnumerable<string> GenerateStr0()
        {
            string[] consosnants = new string[]
                { "р", "п", "к", "т", "л", "м", "н", "д", "ш", "ж", "с", "з", "ф", "г", "в", "б", "х", "ц", "ч", "щ" };

            string[] vowels = new string[] { "а", "о", "у", "э", "ы", "и", "я", "ю", "е", "ё" };
            
            foreach (var v in vowels)
            {
                yield return $"{v}{v}{v}{v}";
            }

            foreach (var c in consosnants)
            {
                foreach (var v in vowels)
                {
                    yield return $"{c}{v}{v}{v}";
                }
            }
        }
    }
}