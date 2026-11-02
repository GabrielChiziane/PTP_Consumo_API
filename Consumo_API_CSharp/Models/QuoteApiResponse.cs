using System.Text.Json.Serialization;

namespace Consumo_API_CSharp.Models;

public class QuoteApiResponse
{
    [JsonPropertyName("quotes")]
    public List<Quote> Quotes { get; set; } = new();
}
