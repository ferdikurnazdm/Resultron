# Resultron

**Resultron** is a lightweight, fluent, and functional-style Result library for C#.  
It provides `Result` and `Result<T>` types with **Try, Map, Bind, and Match** operations,  
enabling safe error handling, fluent pipelines, and exception-free code.

[![NuGet](https://img.shields.io/nuget/v/Resultron.svg)](https://www.nuget.org/packages/Resultron)
[![License](https://img.shields.io/badge/license-MIT-blue.svg)](LICENSE)
[![GitHub](https://img.shields.io/badge/github-ferdikurnazdm%2FResultron-black)](https://github.com/ferdikurnazdm/Resultron)
[![Build](https://img.shields.io/github/actions/workflow/status/ferdikurnazdm/Resultron/ci.yml?branch=master)](https://github.com/ferdikurnazdm/Resultron/actions)

---

## Why Resultron?

Exceptions can make control flow unpredictable and expensive.  
Resultron provides a functional approach to error handling with explicit success and failure states.

## Features

- **Fluent API** with `Try`, `TryAsync`, `Map`, `Bind`, `Match`
- **Non-generic** and **generic** `Result` types
- **Exception-safe** operations with `Result.Try` and `Result.TryAsync`
- Fully **chainable pipelines** for functional programming style
- **Structured errors** with `Code` and `Description`

---

## Installation

You can install the package via NuGet:

```bash
dotnet add package Resultron
```

## Quick Example

```csharp
string input = "10";

var message =
    Result.Try(() => int.Parse(input))
        .Bind(x => Result.Success(x * 2))
        .Match(
            onSuccess: value => $"Result: {value}",
            onFailure: error => $"Error: {error.Description}"
        );

Console.WriteLine(message); // Result: 20
```

## Chaining Example

```csharp
var result = Result.Success()
    .Bind(ValidateName)
    .Bind(ValidateEmail)
    .Bind(SaveUser)
    .Match(
        onSuccess: () => "User created successfully.",
        onFailure: error => $"Failed: {error.Description}"
    );
```

## Async Example

```csharp
var result = await Result.TryAsync(async () =>
{
    await _repository.SaveAsync(entity);
});

result.Match(
    onSuccess: () => Console.WriteLine("Saved."),
    onFailure: error => Console.WriteLine($"Error: {error.Description}")
);
```

---

## Documentation

See the full [documentation](https://github.com/ferdikurnazdm/Resultron/blob/master/README.md).

## Contributing

Please read [CONTRIBUTING.md](CONTRIBUTING.md) before submitting pull requests.

## Changelog

See [CHANGELOG.md](CHANGELOG.md) for release history.

## License

MIT License — see [LICENSE](LICENSE).

## Code of Conduct

See [CODE_OF_CONDUCT.md](CODE_OF_CONDUCT.md).

## Security

See [SECURITY.md](SECURITY.md).