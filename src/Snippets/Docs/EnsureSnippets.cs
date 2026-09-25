using Resultron;

namespace Snippets.Docs;

public static class EnsureSnippets
{
    public static void ResultEnsure()
    {
        Result result = Result.Success();

        Result ensured = result.Ensure(
            () => true,
            new Error(
                "validation.failed",
                "Validation failed."));

        Console.WriteLine(ensured.IsSuccess);
    }

    public static void ResultEnsureFailure()
    {
        Result result = Result.Success();

        Result ensured = result.Ensure(
            () => false,
            new Error(
                "validation.failed",
                "Validation failed."));

        Console.WriteLine(ensured.IsFailure);
        Console.WriteLine(ensured.Error.Code);
    }

    public static void ResultOfTEnsure()
    {
        Result<int> result = Result<int>.Success(42);

        Result<int> ensured = result.Ensure(
            value => value > 0,
            new Error(
                "value.invalid",
                "Value must be greater than zero."));

        Console.WriteLine(ensured.Value);
    }

    public static void ResultOfTEnsureFailure()
    {
        Result<int> result = Result<int>.Success(-1);

        Result<int> ensured = result.Ensure(
            value => value > 0,
            new Error(
                "value.invalid",
                "Value must be greater than zero."));

        Console.WriteLine(ensured.IsFailure);
        Console.WriteLine(ensured.Error.Code);
    }

    public static async Task ResultEnsureAsync()
    {
        Result result = Result.Success();

        Result ensured = await result.EnsureAsync(
            async () =>
            {
                await Task.Delay(10);

                return true;
            },
            new Error(
                "validation.failed",
                "Validation failed."));

        Console.WriteLine(ensured.IsSuccess);
    }

    public static async Task ResultOfTEnsureAsync()
    {
        Result<int> result = Result<int>.Success(42);

        Result<int> ensured = await result.EnsureAsync(
            async value =>
            {
                await Task.Delay(10);

                return value > 0;
            },
            new Error(
                "value.invalid",
                "Value must be greater than zero."));

        Console.WriteLine(ensured.Value);
    }

    public static async Task TaskResultEnsure()
    {
        Task<Result> resultTask =
            Task.FromResult(Result.Success());

        Result ensured = await resultTask.EnsureAsync(
            () => true,
            new Error(
                "validation.failed",
                "Validation failed."));

        Console.WriteLine(ensured.IsSuccess);
    }

    public static async Task TaskResultEnsureAsync()
    {
        Task<Result> resultTask =
            Task.FromResult(Result.Success());

        Result ensured = await resultTask.EnsureAsync(
            async () =>
            {
                await Task.Delay(10);

                return true;
            },
            new Error(
                "validation.failed",
                "Validation failed."));

        Console.WriteLine(ensured.IsSuccess);
    }

    public static async Task TaskResultOfTEnsure()
    {
        Task<Result<int>> resultTask =
            Task.FromResult(Result<int>.Success(42));

        Result<int> ensured = await resultTask.EnsureAsync(
            value => value > 0,
            new Error(
                "value.invalid",
                "Value must be greater than zero."));

        Console.WriteLine(ensured.Value);
    }

    public static async Task TaskResultOfTEnsureAsync()
    {
        Task<Result<int>> resultTask =
            Task.FromResult(Result<int>.Success(42));

        Result<int> ensured = await resultTask.EnsureAsync(
            async value =>
            {
                await Task.Delay(10);

                return value > 0;
            },
            new Error(
                "value.invalid",
                "Value must be greater than zero."));

        Console.WriteLine(ensured.Value);
    }

    public static Result<int> ValidateValue()
    {
        return Result<int>
            .Success(42)
            .Ensure(
                value => value > 0,
                new Error(
                    "value.invalid",
                    "Value must be greater than zero."));
    }

    public static Result<int> ChainedEnsure()
    {
        return Result<int>
            .Success(42)
            .Ensure(
                value => value > 0,
                new Error(
                    "value.negative",
                    "Value must be positive."))
            .Ensure(
                value => value < 100,
                new Error(
                    "value.too_large",
                    "Value must be less than 100."));
    }
}