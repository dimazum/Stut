using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using stutvds.Models.VoiceAnalizerDto;

namespace stutvds.Clients;

public class VoiceAnalyzerClient
{
    private readonly HttpClient _http;

    public VoiceAnalyzerClient(HttpClient http)
    {
        _http = http;
    }

    public async Task<VoiceAnalysisResult?> AnalyzeAsync(
        Stream audioStream,
        string fileName)
    {
        using var content = new MultipartFormDataContent();

        var fileContent = new StreamContent(audioStream);
        fileContent.Headers.ContentType = new MediaTypeHeaderValue("audio/webm");

        content.Add(fileContent, "file", fileName);

        var response = await _http.PostAsync("/analyze", content);
        response.EnsureSuccessStatusCode();

        var stream = await response.Content.ReadAsStreamAsync();

        return await JsonSerializer.DeserializeAsync<VoiceAnalysisResult>(
            stream,
            new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            }
 
        );
    }
}