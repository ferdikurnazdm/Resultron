using Resultron;

namespace Snippets.Docs;

public static class MaybeSnippets
{
    public static void CreateSome()
    {
        Maybe<string> maybe = Maybe<string>.Some("John");

        Console.WriteLine(maybe.HasValue);

        Console.WriteLine(maybe.Value);
    }

    public static void CreateNone()
    {
        Maybe<string> maybe = Maybe<string>.None();

        Console.WriteLine(maybe.HasNoValue);
    }

    public static void CreateFromValue()
    {
        string? value = "John";

        Maybe<string> maybe = Maybe<string>.From(value);

        Console.WriteLine(maybe.HasValue);
    }

    public static void CreateFromNull()
    {
        string? value = null;

        Maybe<string> maybe = Maybe<string>.From(value);

        Console.WriteLine(maybe.HasNoValue);
    }

    public static void ImplicitConversion()
    {
        Maybe<string> some = "John";

        Maybe<string> none = null;

        Console.WriteLine(some.HasValue);
        
        Console.WriteLine(none.HasNoValue);
    }

    public static void GetValue()
    {
        Maybe<string> maybe = Maybe<string>.Some("John");

        string value = maybe.Value;

        Console.WriteLine(value);
    }

    public static void GetValueOrDefault()
    {
        Maybe<string> maybe = Maybe<string>.None();

        string? value = maybe.GetValueOrDefault();

        Console.WriteLine(value);
    }

    public static void GetValueOrDefaultWithFallback()
    {
        Maybe<string> maybe = Maybe<string>.None();

        string value = maybe.GetValueOrDefault("Unknown");

        Console.WriteLine(value);
    }

    public static void GetValueOrElse()
    {
        Maybe<string> maybe = Maybe<string>.None();

        string value = maybe.GetValueOrElse(
            () => "Generated value");

        Console.WriteLine(value);
    }

    public static void Map()
    {
        Maybe<string> maybe = Maybe<string>.Some("John");

        Maybe<int> length = maybe.Map(
            value => value.Length);

        Console.WriteLine(length.Value);
    }

    public static void Bind()
    {
        Maybe<string> maybe = Maybe<string>.Some("42");

        Maybe<int> number = maybe.Bind(value =>
            int.TryParse(value, out int result)
                ? Maybe<int>.Some(result)
                : Maybe<int>.None());

        Console.WriteLine(number.Value);
    }

    public static void Match()
    {
        Maybe<string> maybe = Maybe<string>.Some("John");

        string message = maybe.Match(
            some: value => $"Hello {value}",
            none: () => "No value");

        Console.WriteLine(message);
    }

    public static void IfSome()
    {
        Maybe<string> maybe = Maybe<string>.Some("John");

        maybe.IfSome(value =>
            Console.WriteLine($"Value: {value}"));
    }

    public static void IfNone()
    {
        Maybe<string> maybe = Maybe<string>.None();

        maybe.IfNone(() =>
            Console.WriteLine("No value"));
    }
}