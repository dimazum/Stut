using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace stutvds.Integrations;

public class PollinationsIS
{
    public async Task<string> GetText()
    {
        string query = "Как технологии изменят будущее образования";
        string url = $"https://text.pollinations.ai/{Uri.EscapeDataString(query)}";

        using HttpClient client = new HttpClient();
        HttpResponseMessage response = await client.GetAsync(url);
        response.EnsureSuccessStatusCode();

        string result = await response.Content.ReadAsStringAsync();
        
        return result;
    }
    
    public async Task<string> GetDubaiFact()
    {
        // Формируем запрос: текст про Дубай, минимум 500 слов
        string prompt = "Напиши интересный факт про Беларусь, минимум 500 слов. Сделай текст увлекательным, с деталями и историей.";

        // Кодируем URL
        string url = $"https://text.pollinations.ai/{Uri.EscapeDataString(prompt)}";

        using HttpClient client = new HttpClient();
        HttpResponseMessage response = await client.GetAsync(url);
        response.EnsureSuccessStatusCode();

        string result = await response.Content.ReadAsStringAsync();

        // Проверяем длину текста (в словах)
        int wordCount = result.Split(' ', StringSplitOptions.RemoveEmptyEntries).Length;

        if (wordCount < 500)
        {
            // Если текста меньше 500 слов — можно повторно запросить или сгенерировать дополнительный текст
            string extraPrompt = "Продолжи текст, чтобы получилось не менее 500 слов про Беларусь, добавь новые интересные факты.";
            string extraUrl = $"https://text.pollinations.ai/{Uri.EscapeDataString(extraPrompt)}";

            HttpResponseMessage extraResponse = await client.GetAsync(extraUrl);
            extraResponse.EnsureSuccessStatusCode();

            string extraResult = await extraResponse.Content.ReadAsStringAsync();
            result += "\n\n" + extraResult;
        }

        return result;
    }

}