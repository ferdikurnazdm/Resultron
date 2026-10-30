# Resultron

**Resultron** is a lightweight, fluent, and functional-style Result library for C#.  
It provides `Result` and `Result<T>` types with **Try, Map, Bind, and Match** operations,  
enabling explicit error handling, composable pipelines, and predictable application flow.


[![NuGet Version](https://img.shields.io/nuget/v/Resultron.svg)](https://www.nuget.org/packages/Resultron)
[![NuGet Downloads](https://img.shields.io/nuget/dt/Resultron?label=Core%20Downloads)](https://www.nuget.org/packages/Resultron)
[![NuGet Version](https://img.shields.io/nuget/v/Resultron.FluentAssertions.svg)](https://www.nuget.org/packages/Resultron.FluentAssertions)
[![NuGet Downloads](https://img.shields.io/nuget/dt/Resultron.FluentAssertions?label=FluentAssertions%20Downloads)](https://www.nuget.org/packages/Resultron.FluentAssertions)
[![License](https://img.shields.io/badge/license-MIT-blue.svg)](LICENSE)
[![GitHub](https://img.shields.io/badge/GitHub-ferdikurnazdm%2FResultron-black?logo=github)](https://github.com/ferdikurnazdm/Resultron)
[![Core Build](https://img.shields.io/github/actions/workflow/status/ferdikurnazdm/Resultron/nuget.yml?branch=main&label=Core%20Build)](https://github.com/ferdikurnazdm/Resultron/actions/workflows/nuget.yml)
[![FluentAssertions Build](https://img.shields.io/github/actions/workflow/status/ferdikurnazdm/Resultron/fluentassertions-nuget.yml?branch=main&label=FluentAssertions%20Build)](https://github.com/ferdikurnazdm/Resultron/actions/workflows/fluentassertions-nuget.yml)




## Why Resultron?

Exceptions can make control flow unpredictable and expensive.  
Resultron provides a functional approach to error handling with explicit success and failure states.


## Features

- **Fluent API** with `Try`, `TryAsync`, `Map`, `Bind`, `Match`
- **Non-generic** and **generic** `Result` types
- **Exception-safe** operations with `Result.Try` and `Result.TryAsync`
- **Composable pipelines** for clean and maintainable code
- **Structured errors** with `Code` and `Description`
- **FluentAssertions** integration for expressive and readable unit tests


## Packages

| Package | Description |
|:---|:---|
| [Resultron](https://www.nuget.org/packages/Resultron) | Core Result library for functional error handling |
| [Resultron.FluentAssertions](https://www.nuget.org/packages/Resultron.FluentAssertions) | FluentAssertions integration for testing Result instances |


## Installation

### Core library

Install the core package:

```bash
dotnet add package Resultron
```

### FluentAssertions integration

For FluentAssertions integration:

```bash
dotnet add package Resultron.FluentAssertions
```

## Quick Example

```csharp
string input = "10";

var message =
    Result.Try(() => int.Parse(input))
        .Map(x => x * 2)
        .Match(
            onSuccess: value => $"Result: {value}",
            onFailure: error => $"Error: {error.Description}"
        );

Console.WriteLine(message); // Result: 20
```

## Documentation

See the full [documentation](https://github.com/ferdikurnazdm/Resultron/wiki).

## Contributing

Please read [CONTRIBUTING.md](https://github.com/ferdikurnazdm/Resultron/blob/main/CONTRIBUTING.md) before submitting pull requests.

## Changelog

See [CHANGELOG.md](https://github.com/ferdikurnazdm/Resultron/blob/main/CHANGELOG.md) for release history.

## License

MIT License — see [LICENSE](https://github.com/ferdikurnazdm/Resultron/blob/main/LICENSE).

## Code of Conduct

See [CODE_OF_CONDUCT.md](https://github.com/ferdikurnazdm/Resultron/blob/main/CODE_OF_CONDUCT.md).

## Security

See [SECURITY.md](https://github.com/ferdikurnazdm/Resultron/blob/main/SECURITY.md).