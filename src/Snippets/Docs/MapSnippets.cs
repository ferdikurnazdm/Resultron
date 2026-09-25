using Resultron;

namespace Snippets.Docs;

public static class MapSnippets
{
    public static void ResultToValue()
    {
        Result source = Result.Success();

        Result<int> result = source.Map(
            () => 42);

        Console.WriteLine(result.Value);
    }

    public static void ResultOfTToValue()
    {
        Result<int> source = Result<int>.Success(42);

        Result<string> result = source.Map(
            value => value.ToString());

        Console.WriteLine(result.Value);
    }

    public static void ChainedMap()
    {
        Result<string> result = Result<int>
            .Success(42)
            .Map(value => value * 2)
            .Map(value => $"Value: {value}");

        Console.WriteLine(result.Value);
    }

    public static void ShortCircuitFailure()
    {
        Result<int> source = Result<int>.Failure(
            new Error(
                "operation.failed",
                "The operation failed."));

        Result<string> result = source.Map(
            value => value.ToString());

        Console.WriteLine(result.IsFailure);
        Console.WriteLine(result.Error.Code);
    }

    public static async Task ResultToValueAsync()
    {
        Result source = Result.Success();

        Result<int> result = await source.MapAsync(
            async () =>
            {
                await Task.Delay(10);

                return 42;
            });

        Console.WriteLine(result.Value);
    }

    public static async Task ResultOfTToValueAsync()
    {
        Result<int> source = Result<int>.Success(42);

        Result<string> result = await source.MapAsync(
            async value =>
            {
                await Task.Delay(10);

                return value.ToString();
            });

        Console.WriteLine(result.Value);
    }

    public static async Task TaskResultToValue()
    {
        Task<Result> source =
            Task.FromResult(Result.Success());

        Result<int> result = await source.MapAsync(
            () => 42);

        Console.WriteLine(result.Value);
    }

    public static async Task TaskResultToValueAsync()
    {
        Task<Result> source =
            Task.FromResult(Result.Success());

        Result<int> result = await source.MapAsync(
            async () =>
            {
                await Task.Delay(10);

                return 42;
            });

        Console.WriteLine(result.Value);
    }

    public static async Task TaskResultOfTToValue()
    {
        Task<Result<int>> source =
            Task.FromResult(Result<int>.Success(42));

        Result<string> result = await source.MapAsync(
            value => value.ToString());

        Console.WriteLine(result.Value);
    }

    public static async Task TaskResultOfTToValueAsync()
    {
        Task<Result<int>> source =
            Task.FromResult(Result<int>.Success(42));

        Result<string> result = await source.MapAsync(
            async value =>
            {
                await Task.Delay(10);

                return value.ToString();
            });

        Console.WriteLine(result.Value);
    }
}