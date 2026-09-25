using Resultron;

namespace Snippets.Docs;

public static class TapSnippets
{
    public static void ResultTap()
    {
        Result result = Result.Success();

        Result afterTap = result.Tap(
            () => Console.WriteLine(
                "Operation completed successfully."));

        Console.WriteLine(afterTap.IsSuccess);
    }

    public static void ResultOfTTap()
    {
        Result<int> result = Result<int>.Success(42);

        Result<int> afterTap = result.Tap(
            value => Console.WriteLine(
                $"Value: {value}"));

        Console.WriteLine(afterTap.Value);
    }

    public static void FluentTap()
    {
        Result<int> result = Result<int>
            .Success(42)
            .Tap(value =>
                Console.WriteLine($"Value: {value}"))
            .Tap(value =>
                Console.WriteLine($"Double: {value * 2}"));

        Console.WriteLine(result.Value);
    }

    public static async Task ResultTapAsync()
    {
        Result result = Result.Success();

        Result afterTap = await result.TapAsync(
            async () =>
            {
                await Task.Delay(10);

                Console.WriteLine(
                    "Operation completed successfully.");
            });

        Console.WriteLine(afterTap.IsSuccess);
    }

    public static async Task ResultOfTTapAsync()
    {
        Result<int> result = Result<int>.Success(42);

        Result<int> afterTap = await result.TapAsync(
            async value =>
            {
                await Task.Delay(10);

                Console.WriteLine($"Value: {value}");
            });

        Console.WriteLine(afterTap.Value);
    }

    public static async Task TaskResultTap()
    {
        Task<Result> resultTask =
            Task.FromResult(Result.Success());

        Result result = await resultTask.TapAsync(
            () => Console.WriteLine(
                "Operation completed successfully."));

        Console.WriteLine(result.IsSuccess);
    }

    public static async Task TaskResultTapAsync()
    {
        Task<Result> resultTask =
            Task.FromResult(Result.Success());

        Result result = await resultTask.TapAsync(
            async () =>
            {
                await Task.Delay(10);

                Console.WriteLine(
                    "Operation completed successfully.");
            });

        Console.WriteLine(result.IsSuccess);
    }

    public static async Task TaskResultOfTTap()
    {
        Task<Result<int>> resultTask =
            Task.FromResult(Result<int>.Success(42));

        Result<int> result = await resultTask.TapAsync(
            value => Console.WriteLine(
                $"Value: {value}"));

        Console.WriteLine(result.Value);
    }

    public static async Task TaskResultOfTTapAsync()
    {
        Task<Result<int>> resultTask =
            Task.FromResult(Result<int>.Success(42));

        Result<int> result = await resultTask.TapAsync(
            async value =>
            {
                await Task.Delay(10);

                Console.WriteLine($"Value: {value}");
            });

        Console.WriteLine(result.Value);
    }
}