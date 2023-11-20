using System.Text.Json;
using OllamaTot.Dtos;
using System.Text;

namespace OllamaTot.Utilities
{
  public class Ollama
  {
    private readonly HttpClient _client;
    private readonly OllamaRequest _requestData;

    public Ollama(HttpClient client, OllamaRequest requestData)
    {
      _client = client;
      _requestData = requestData;
    }

    public async Task<string> Generate(string serverIP)
    {
      var ollamaApiUrl = $"http://{serverIP}:11434/api/generate";
      var requestContent = new StringContent(JsonSerializer.Serialize(_requestData));

      var response = await _client.PostAsync(ollamaApiUrl, requestContent);
      if (!response.IsSuccessStatusCode)
      {
        throw new Exception("Ollama request failed");
      }
      var responseStream = await response.Content.ReadAsStreamAsync();
      using var reader = new StreamReader(responseStream);
      var fullResponse = new StringBuilder();
      while (!reader.EndOfStream)
      {
        var line = await reader.ReadLineAsync();
        if (!string.IsNullOrEmpty(line))
        {
          var ollamaResponse = JsonSerializer.Deserialize<OllamaResponse>(line);
          if (ollamaResponse == null)
          {
            throw new JsonException("Failed to deserialize Ollama response");
          }
          fullResponse.Append(ollamaResponse.Response);
        }
      }
      return fullResponse.ToString();
    }
  }
}
