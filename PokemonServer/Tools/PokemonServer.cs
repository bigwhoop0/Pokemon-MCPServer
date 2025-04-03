using System.ComponentModel;
using System.Net.Http.Json;
using System.Text.Json;
using ModelContextProtocol.Server;

namespace PokemonMCP.PokemonServer.Tools;

[McpServerToolType]
public class PokemonServer
{
    [McpServerTool, Description("Get information about a Pokémon by ID.")]
    public static async Task<string> GetPokemonById(
        HttpClient client,
        [Description("The ID of the Pokémon.")] int id)
    {
        try
        {
            using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(8));
            var pokemon = await client.GetFromJsonAsync<JsonElement>($"pokemon/{id}", cts.Token);
            return FormatPokemonData(pokemon);
        }
        catch (Exception ex)
        {
            return $"Erreur lors de la recherche du Pokémon #{id}: {ex.Message}";
        }
    }

    [McpServerTool, Description("Get information about a Pokémon by name.")]
    public static async Task<string> GetPokemonByName(
        HttpClient client,
        [Description("The name of the Pokémon.")] string name)
    {
        try
        {
            using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(8));
            var pokemon = await client.GetFromJsonAsync<JsonElement>($"pokemon/{name}", cts.Token);
            
            return FormatPokemonData(pokemon);
        }
        catch (TaskCanceledException)
        {
            return $"Délai dépassé lors de la recherche de '{name}'.";
        }
        catch (HttpRequestException ex)
        {
            return $"Erreur lors de la recherche de '{name}': {ex.Message}";
        }
        catch (Exception)
        {
            return $"Erreur interne lors de la recherche de '{name}'.";
        }
    }

    [McpServerTool, Description("Get Pokémon by generation.")]
    public static async Task<string> GetPokemonByGeneration(
        HttpClient client,
        [Description("The generation number (1-8).")] int generation)
    {
        try
        {
            using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(8));
            var pokemons = await client.GetFromJsonAsync<JsonElement[]>($"pokemon/generation/{generation}", cts.Token);

            if (pokemons == null || pokemons.Length == 0)
                return $"Aucun Pokémon trouvé pour la génération {generation}.";

            return $"Trouvé {pokemons.Length} Pokémon de génération {generation}:\n" +
                   string.Join("\n", pokemons.Take(10).Select(p =>
                       $"#{p.GetProperty("id").GetInt32()}: {p.GetProperty("name").GetString()}"));
        }
        catch (Exception ex)
        {
            return $"Erreur lors de la recherche des Pokémon de génération {generation}: {ex.Message}";
        }
    }

    [McpServerTool, Description("Get Pokémon by dual types.")]
    public static async Task<string> GetPokemonByDualTypes(
        HttpClient client,
        [Description("First type (e.g., Eau, Feu).")] string type1,
        [Description("Second type (e.g., Vol, Combat).")] string type2)
    {
        try
        {
            using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(8));
            var pokemons = await client.GetFromJsonAsync<JsonElement[]>($"pokemon/types/{type1}/{type2}", cts.Token);

            if (pokemons == null || pokemons.Length == 0)
                return $"Aucun Pokémon trouvé avec les types {type1} et {type2}.";

            return $"Trouvé {pokemons.Length} Pokémon avec les types {type1} et {type2}:\n" +
                   string.Join("\n", pokemons.Take(10).Select(p =>
                       $"#{p.GetProperty("id").GetInt32()}: {p.GetProperty("name").GetString()}"));
        }
        catch (Exception ex)
        {
            return $"Erreur lors de la recherche des Pokémon de types {type1} et {type2}: {ex.Message}";
        }
    }

    [McpServerTool, Description("Generate a random team of Pokémon.")]
    public static async Task<string> GetRandomTeam(HttpClient client)
    {
        try
        {
            using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(8));
            var team = await client.GetFromJsonAsync<JsonElement[]>("random/team", cts.Token);

            if (team == null || team.Length == 0)
                return "Échec de la génération d'une équipe aléatoire.";

            return "Votre équipe Pokémon aléatoire:\n" +
                   string.Join("\n", team.Select(p =>
                       $"#{p.GetProperty("id").GetInt32()}: {p.GetProperty("name").GetString()} - Types: {GetTypesAsString(p.GetProperty("apiTypes"))}"));
        }
        catch (Exception ex)
        {
            return $"Erreur lors de la génération d'une équipe aléatoire: {ex.Message}";
        }
    }

    [McpServerTool, Description("Generate a balanced team suggestion.")]
    public static async Task<string> GetBalancedTeamSuggestion(HttpClient client)
    {
        try
        {
            using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(8));
            var team = await client.GetFromJsonAsync<JsonElement[]>("random/team/suggest", cts.Token);

            if (team == null || team.Length == 0)
                return "Échec de la génération d'une équipe équilibrée.";

            return "Suggestion d'équipe Pokémon équilibrée:\n" +
                   string.Join("\n", team.Select(p =>
                       $"#{p.GetProperty("id").GetInt32()}: {p.GetProperty("name").GetString()} - Types: {GetTypesAsString(p.GetProperty("apiTypes"))}"));
        }
        catch (Exception ex)
        {
            return $"Erreur lors de la génération d'une équipe équilibrée: {ex.Message}";
        }
    }

    [McpServerTool, Description("Get Pokémon with weakness to a specific type.")]
    public static async Task<string> GetPokemonWithWeakness(
        HttpClient client,
        [Description("The type (e.g., Fée, Feu).")] string type)
    {
        try
        {
            using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(8));
            var pokemons = await client.GetFromJsonAsync<JsonElement[]>($"pokemon/type/weakness/{type}", cts.Token);

            if (pokemons == null || pokemons.Length == 0)
                return $"Aucun Pokémon trouvé avec une faiblesse au type {type}.";

            return $"Trouvé {pokemons.Length} Pokémon avec une faiblesse au type {type}. Voici les 10 premiers:\n" +
                   string.Join("\n", pokemons.Take(10).Select(p =>
                       $"#{p.GetProperty("id").GetInt32()}: {p.GetProperty("name").GetString()}"));
        }
        catch (Exception ex)
        {
            return $"Erreur lors de la recherche des Pokémon avec faiblesse au type {type}: {ex.Message}";
        }
    }

    [McpServerTool, Description("Get all available Pokémon types.")]
    public static async Task<string> GetAllTypes(HttpClient client)
    {
        try
        {
            using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(8));
            var types = await client.GetFromJsonAsync<JsonElement[]>("types", cts.Token);

            if (types == null || types.Length == 0)
                return "Échec de la récupération des types de Pokémon.";

            return "Types de Pokémon disponibles:\n" +
                   string.Join("\n", types.Select(t =>
                       $"{t.GetProperty("name").GetString()}"));
        }
        catch (Exception ex)
        {
            return $"Erreur lors de la récupération des types de Pokémon: {ex.Message}";
        }
    }

    // Formatage des données Pokémon
    private static string FormatPokemonData(JsonElement pokemon)
    {
        var id = pokemon.GetProperty("id").GetInt32();
        var name = pokemon.GetProperty("name").GetString();
        var types = GetTypesAsString(pokemon.GetProperty("apiTypes"));
        var stats = pokemon.GetProperty("stats");
        var image = pokemon.GetProperty("image").GetString();
        var sprite = pokemon.GetProperty("sprite").GetString();

        return $"""
                #{id}: {name}
                Types: {types}
                Images:
                  Artwork: {image}
                  Sprite: {sprite}
                Stats:
                  HP: {stats.GetProperty("HP").GetInt32()}
                  Attack: {stats.GetProperty("attack").GetInt32()}
                  Defense: {stats.GetProperty("defense").GetInt32()}
                  Sp. Attack: {stats.GetProperty("special_attack").GetInt32()}
                  Sp. Defense: {stats.GetProperty("special_defense").GetInt32()}
                  Speed: {stats.GetProperty("speed").GetInt32()}
                """;
    }

    // Extraction des types sous forme de chaîne
    private static string GetTypesAsString(JsonElement typesElement)
    {
        return string.Join(", ", typesElement.EnumerateArray().Select(t => t.GetProperty("name").GetString()));
    }
}