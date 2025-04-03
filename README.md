# Pokemon MCP Server

A Model Context Protocol server that provides tools for accessing and interacting with Pokémon data. This project uses the [PokebuilAPI](https://pokebuildapi.fr/api/v1) to retrieve information about Pokémon and helps with team building and Pokémon research.

## Features

The server provides the following tools:

- **Get Pokémon information** by ID or name
- **List Pokémon by generation**
- **Search Pokémon by dual types**
- **Generate random Pokémon teams**
- **Get balanced team suggestions**
- **Find Pokémon with specific type weaknesses**
- **List all available Pokémon types**

## Prerequisites

- .NET 9.0 SDK
- An internet connection to access the PokebuilAPI

## Getting Started

1. Clone this repository
2. Build the project:
   ```bash
   dotnet build
   ```
3. Run the application:
   ```bash
   dotnet run --project PokemonMCP/PokemonMCP.csproj
   ```

## Using the MCP Server

Once the server is running, you can interact with it using the Model Context Protocol. Here are some examples:

### Get information about a Pokémon by ID
```
GetPokemonById 25
```

### Get information about a Pokémon by name
```
GetPokemonByName pikachu
```

### List Pokémon from a specific generation
```
GetPokemonByGeneration 1
```

### Find Pokémon with dual types
```
GetPokemonByDualTypes Eau Vol
```

### Generate a random team
```
GetRandomTeam
```

### Get a balanced team suggestion
```
GetBalancedTeamSuggestion
```

### Find Pokémon with weakness to a specific type
```
GetPokemonWithWeakness Feu
```

### List all available Pokémon types
```
GetAllTypes
```

## API Reference

This project uses the [PokebuilAPI](https://pokebuildapi.fr/api/v1) to fetch Pokémon data. All requests are subject to the API's rate limits and terms of service.

## License

This project is licensed under the MIT License - see the LICENSE file for details.

## Acknowledgments

- [PokebuilAPI](https://pokebuildapi.fr/) for providing the Pokémon data
- [Model Context Protocol](https://github.com/microsoft/modelcontextprotocol) for the MCP server implementation
