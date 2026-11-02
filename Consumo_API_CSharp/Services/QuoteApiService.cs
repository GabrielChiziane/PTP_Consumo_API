using System.Text.Json;
using Consumo_API_CSharp.Models;

namespace Consumo_API_CSharp.Services;

public class QuoteApiService
{
    private const string ApiUrl = "https://dummyjson.com/quotes";
    private static readonly HttpClient Client = new()
    {
        Timeout = TimeSpan.FromSeconds(15)
    };

    public async Task<List<Quote>> GetQuotesAsync()
    {
        using var response = await Client.GetAsync(ApiUrl);

        if (!response.IsSuccessStatusCode)
        {
            throw new HttpRequestException(
                $"A API respondeu com o estado HTTP {(int)response.StatusCode} ({response.ReasonPhrase}).");
        }

        await using var responseStream = await response.Content.ReadAsStreamAsync();
        var result = await JsonSerializer.DeserializeAsync<QuoteApiResponse>(responseStream);

        if (result?.Quotes is null)
        {
            throw new JsonException("A resposta da API não contém a lista de citações esperada.");
        }

        return result.Quotes;
    }
}
