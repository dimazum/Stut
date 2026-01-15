using System;
using System.Collections.Generic;
using System.Linq;

public class TriggerGeneratorService
{
    private static readonly char[] Front = { 'п','б','м','ф','в' };        
    private static readonly char[] Middle = { 'н','т','д','л','р' };       
    private static readonly char[] Back = { 'к','г','х' };                  
    private static readonly char[] Vowels = { 'а','о','у','э','и','е','ы','ю','я' };

    private static readonly Random rnd = new Random();

    /// <summary>
    /// Генерирует 10 читаемых сложных триггеров 3–4 буквы для слова
    /// </summary>
    public List<string> GenerateTriggers(string word, int numTriggers = 10)
    {
        if (string.IsNullOrEmpty(word)) return new List<string>();

        bool startsWithBack = Back.Contains(char.ToLower(word[0]));
        var triggers = new HashSet<string>();
        int attempts = 0;
        int maxAttempts = numTriggers * 10;

        while (triggers.Count < numTriggers && attempts < maxAttempts)
        {
            attempts++;
            string trigger = GenerateReadableTrigger(startsWithBack);
            string line = $"{trigger} {word}, {trigger} {word}, {trigger} {word}, {trigger} {word}";
            triggers.Add(line);
        }

        return triggers.ToList();
    }

    /// <summary>
    /// Генерирует один читаемый триггер 3–4 буквы
    /// </summary>
    private string GenerateReadableTrigger(bool startsWithBack)
    {
        int length = 3 + (rnd.NextDouble() < 0.5 ? 1 : 0); // 3 или 4 буквы
        string trigger = "";
        char lastChar = '\0';

        for (int i = 0; i < length - 1; i++)
        {
            char nextChar;

            if (i % 2 == 0) // согласный
            {
                nextChar = RandomChoice(Front.Concat(Middle).ToArray()); // согласные из читаемых зон
                while (nextChar == lastChar) // избегаем повторов
                    nextChar = RandomChoice(Front.Concat(Middle).ToArray());
            }
            else // гласная
            {
                nextChar = RandomChoice(Vowels);
            }

            trigger += nextChar;
            lastChar = nextChar;
        }

        // Конец триггера
        if (startsWithBack)
        {
            // если слово начинается с Back, конец = Front+Middle
            trigger += RandomChoice(Front.Concat(Middle).ToArray());
        }
        else
        {
            // обычно конец = Back
            trigger += RandomChoice(Back);
        }

        return trigger;
    }

    private static char RandomChoice(char[] options) => options[rnd.Next(options.Length)];
}
