using Resultron;

namespace Snippets.Docs;

public static class BindSnippets
{
    public static void ResultToResult()
    {
        Result source = Result.Success();

        Result result = source.Bind(
            () => Result.Success());

        Console.WriteLine(result.IsSuccess);
    }

    public static void ResultToResultOfT()
    {
        Result source = Result.Success();

        Result<int> result = source.Bind(
            () => Result<int>.Success(42));

        Console.WriteLine(result.Value);
    }

    public static void ResultOfTToResult()
    {
        Result<int> source = Result<int>.Success(42);

        Result result = source.Bind(
            value => value > 0
                ? Result.Success()
                : Result.Failure(
                    new Error(
                        "value.invalid",
                        "Value must be greater than zero.")));

        Console.WriteLine(result.IsSuccess);
    }

    public static void ResultOfTToResultOfT()
    {
        Result<int> source = Result<int>.Success(42);

        Result<string> result = source.Bind(
            value => Result<string>.Success(
                value.ToString()));

        Console.WriteLine(result.Value);
    }

    public static void ShortCircuitFailure()
    {
        Result<int> source = Result<int>.Failure(
            new Error(
                "operation.failed",
                "The operation failed."));

        Result<string> result = source.Bind(
            value => Result<string>.Success(
                value.ToString()));

        Console.WriteLine(result.IsFailure);
        Console.WriteLine(result.Error.Code);
    }

    public static async Task ResultToAsyncResult()
    {
        Result source = Result.Success();

        Result result = await source.BindAsync(
            async () =>
            {
                await Task.Delay(10);

                return Result.Success();
            });

        Console.WriteLine(result.IsSuccess);
    }

    public static async Task ResultOfTToAsyncResultOfT()
    {
        Result<int> source = Result<int>.Success(42);

        Result<string> result = await source.BindAsync(
            async value =>
            {
                await Task.Delay(10);

                return Result<string>.Success(
                    value.ToString());
            });

        Console.WriteLine(result.Value);
    }

    public static async Task TaskResultToResult()
    {
        Task<Result> source =
            Task.FromResult(Result.Success());

        Result result = await source.BindAsync(
            () => Result.Success());

        Console.WriteLine(result.IsSuccess);
    }

    public static async Task TaskResultToAsyncResult()
    {
        Task<Result> source =
            Task.FromResult(Result.Success());

        Result result = await source.BindAsync(
            async () =>
            {
                await Task.Delay(10);

                return Result.Success();
            });

        Console.WriteLine(result.IsSuccess);
    }

    public static async Task TaskResultToResultOfT()
    {
        Task<Result> source =
            Task.FromResult(Result.Success());

        Result<int> result = await source.BindAsync(
            () => Result<int>.Success(42));

        Console.WriteLine(result.Value);
    }

    public static async Task TaskResultToAsyncResultOfT()
    {
        Task<Result> source =
            Task.FromResult(Result.Success());

        Result<int> result = await source.BindAsync(
            async () =>
            {
                await Task.Delay(10);

                return Result<int>.Success(42);
            });

        Console.WriteLine(result.Value);
    }

    public static async Task TaskResultOfTToResultOfT()
    {
        Task<Result<int>> source =
            Task.FromResult(Result<int>.Success(42));

        Result<string> result = await source.BindAsync(
            value => Result<string>.Success(
                value.ToString()));

        Console.WriteLine(result.Value);
    }

    public static async Task TaskResultOfTToAsyncResultOfT()
    {
        Task<Result<int>> source =
            Task.FromResult(Result<int>.Success(42));

        Result<string> result = await source.BindAsync(
            async value =>
            {
                await Task.Delay(10);

                return Result<string>.Success(
                    value.ToString());
            });

        Console.WriteLine(result.Value);
    }

    public static Result<string> ChainedBind()
    {
        return Result<int>
            .Success(42)
            .Bind(value =>
                value > 0
                    ? Result<string>.Success($"Value: {value}")
                    : Result<string>.Failure(
                        new Error(
                            "value.invalid",
                            "Value must be greater than zero.")));
    }

    public static async Task<Result<string>> ChainedBindAsync()
    {
        return await Result<int>
            .Success(42)
            .BindAsync(async value =>
            {
                await Task.Delay(10);

                return Result<string>.Success(
                    $"Value: {value}");
            });
    }

    public static async Task ResultOfTToAsyncResult()
    {
        Result<int> source = Result<int>.Success(42);

        Result result = await source.BindAsync(
            async value =>
            {
                await Task.Delay(10);

                return value > 0
                    ? Result.Success()
                    : Result.Failure(
                        new Error(
                            "value.invalid",
                            "Value must be greater than zero."));
            });

        Console.WriteLine(result.IsSuccess);
    }

}