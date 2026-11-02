using System.Text.Json.Serialization;

namespace Consumo_API_CSharp.Models;

public class Quote
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("quote")]
    public string QuoteText { get; set; } = string.Empty;

    [JsonPropertyName("author")]
    public string Author { get; set; } = string.Empty;
}
