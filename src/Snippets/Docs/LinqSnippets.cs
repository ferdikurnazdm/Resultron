using Resultron;

namespace Snippets.Docs;

public static class LinqSnippets
{
    public static void Select()
    {
        Result<int> result = Result<int>.Success(42);

        Result<string> projected = result.Select(
            value => value.ToString());

        Console.WriteLine(projected.Value);
    }

    public static void SelectFromResult()
    {
        Result result = Result.Success();

        Result<int> projected = result.Select(
            () => 42);

        Console.WriteLine(projected.Value);
    }

    public static void SelectMany()
    {
        Result<int> result = Result<int>.Success(42);

        Result<string> bound = result.SelectMany(
            value => Result<string>.Success(
                value.ToString()));

        Console.WriteLine(bound.Value);
    }

    public static void SelectManyFromResult()
    {
        Result result = Result.Success();

        Result<int> bound = result.SelectMany(
            () => Result<int>.Success(42));

        Console.WriteLine(bound.Value);
    }

    public static void SelectManyWithProjection()
    {
        Result<int> first = Result<int>.Success(10);

        Result<int> result = first.SelectMany(
            value => Result<int>.Success(20),
            (firstValue, secondValue) =>
                firstValue + secondValue);

        Console.WriteLine(result.Value);
    }

    public static void Where()
    {
        Result<int> result = Result<int>.Success(42);

        Result<int> filtered = result.Where(
            value => value > 0,
            new Error(
                "value.invalid",
                "Value must be greater than zero."));

        Console.WriteLine(filtered.Value);
    }

    public static void QuerySyntaxSelect()
    {
        Result<int> source = Result<int>.Success(42);

        Result<string> result =
            from value in source
            select $"Value: {value}";

        Console.WriteLine(result.Value);
    }

    public static void QuerySyntaxSelectMany()
    {
        Result<int> first = Result<int>.Success(10);
        Result<int> second = Result<int>.Success(20);

        Result<int> result =
            from firstValue in first
            from secondValue in second
            select firstValue + secondValue;

        Console.WriteLine(result.Value);
    }

    public static void QuerySyntaxWhereSingleResult()
    {
        Result<int> source = Result<int>.Success(42);

        Result<int> result =
            from value in source
            where value > 0
            select value;

        Console.WriteLine(result.Value);
    }

    public static void QuerySyntaxWhere()
    {
        Result<int> first = Result<int>.Success(10);
        Result<int> second = Result<int>.Success(20);

        Result<int> result =
            from firstValue in first
            from secondValue in second
            where firstValue + secondValue > 20
            select firstValue + secondValue;

        Console.WriteLine(result.Value);
    }
}