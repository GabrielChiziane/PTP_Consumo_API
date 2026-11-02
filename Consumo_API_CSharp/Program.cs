using Consumo_API_CSharp.Models;
using Consumo_API_CSharp.Services;

namespace Consumo_API_CSharp;

public class Program
{
    private static readonly QuoteApiService ApiService = new();
    private static List<Quote> _quotes = new();

    public static async Task Main()
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;
        Console.WriteLine("=== CONSUMO DE API DE CITAÇÕES ===");
        await RefreshQuotesAsync();

        var running = true;
        while (running)
        {
            ShowMenu();
            Console.Write("Escolha uma opção: ");

            switch (Console.ReadLine()?.Trim())
            {
                case "1":
                    ShowQuotes();
                    break;
                case "2":
                    SearchByAuthor();
                    break;
                case "3":
                    ShowRandomQuote();
                    break;
                case "4":
                    await RefreshQuotesAsync();
                    break;
                case "0":
                    running = false;
                    Console.WriteLine("\nObrigado por utilizar o programa. Até breve!");
                    break;
                default:
                    Console.WriteLine("\nOpção inválida. Escolha uma opção entre 0 e 4.");
                    break;
            }

            if (running)
            {
                Console.WriteLine("\nPrima Enter para continuar...");
                Console.ReadLine();
            }
        }
    }

    private static void ShowMenu()
    {
        Console.WriteLine("==================================");
        Console.WriteLine("       MENU DE CITAÇÕES");
        Console.WriteLine("==================================");
        Console.WriteLine("1. Ver todas as citações");
        Console.WriteLine("2. Procurar citações por autor");
        Console.WriteLine("3. Ver uma citação aleatória");
        Console.WriteLine("4. Atualizar dados da API");
        Console.WriteLine("0. Sair");
        Console.WriteLine("----------------------------------");
        Console.WriteLine($"Citações carregadas: {_quotes.Count}");
    }

    private static void ShowQuotes()
    {
        if (!HasQuotes())
        {
            return;
        }

        Console.WriteLine($"\n=== CITAÇÕES ({_quotes.Count}) ===");
        foreach (var quote in _quotes)
        {
            PrintQuote(quote);
        }
    }

    private static void SearchByAuthor()
    {
        if (!HasQuotes())
        {
            return;
        }

        Console.Write("\nEscreva o nome (ou parte do nome) do autor: ");
        var author = Console.ReadLine()?.Trim();

        if (string.IsNullOrWhiteSpace(author))
        {
            Console.WriteLine("O nome do autor não pode estar vazio.");
            return;
        }

        var matches = _quotes
            .Where(quote => quote.Author.Contains(author, StringComparison.OrdinalIgnoreCase))
            .ToList();

        if (matches.Count == 0)
        {
            Console.WriteLine($"Não foram encontradas citações para \"{author}\".");
            return;
        }

        Console.WriteLine($"\n=== RESULTADOS: {matches.Count} ===");
        foreach (var quote in matches)
        {
            PrintQuote(quote);
        }
    }

    private static void ShowRandomQuote()
    {
        if (!HasQuotes())
        {
            return;
        }

        var quote = _quotes[Random.Shared.Next(_quotes.Count)];
        Console.WriteLine("\n=== CITAÇÃO ALEATÓRIA ===");
        PrintQuote(quote);
    }

    private static async Task RefreshQuotesAsync()
    {
        Console.WriteLine("\nA consultar a API DummyJSON...");

        try
        {
            var quotes = await ApiService.GetQuotesAsync();
            _quotes = quotes;
            Console.WriteLine($"{_quotes.Count} citações carregadas com sucesso.");
        }
        catch (HttpRequestException ex)
        {
            Console.WriteLine($"Erro de comunicação com a API: {ex.Message}");
            Console.WriteLine("Verifique a ligação à internet e tente atualizar novamente.");
        }
        catch (TaskCanceledException)
        {
            Console.WriteLine("A ligação à API excedeu o tempo limite de espera.");
        }
        catch (System.Text.Json.JsonException ex)
        {
            Console.WriteLine($"Não foi possível processar a resposta JSON: {ex.Message}");
        }
    }

    private static bool HasQuotes()
    {
        if (_quotes.Count > 0)
        {
            return true;
        }

        Console.WriteLine("Não há citações carregadas. Escolha a opção 4 para tentar novamente.");
        return false;
    }

    private static void PrintQuote(Quote quote)
    {
        Console.WriteLine($"\nID: {quote.Id}");
        Console.WriteLine($"Autor: {quote.Author}");
        Console.WriteLine($"Citação: \"{quote.QuoteText}\"");
        Console.WriteLine("----------------------------------");
    }
}
