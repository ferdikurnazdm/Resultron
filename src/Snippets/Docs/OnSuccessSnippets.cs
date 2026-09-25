using Resultron;

namespace Snippets.Docs;

public static class OnSuccessSnippets
{
    public static void ResultOnSuccess()
    {
        Result result = Result.Success();

        Result afterChain = result.OnSuccess(
            () => Console.WriteLine(
                "Operation completed successfully."));

        Console.WriteLine(afterChain.IsSuccess);
    }

    public static void ResultOfTOnSuccess()
    {
        Result<int> result = Result<int>.Success(42);

        Result<int> afterChain = result.OnSuccess(
            value => Console.WriteLine(
                $"Value: {value}"));

        Console.WriteLine(afterChain.Value);
    }

    public static void FluentChain()
    {
        Result<int> result = Result<int>
            .Success(42)
            .OnSuccess(
                value => Console.WriteLine(
                    $"Value: {value}"));

        Console.WriteLine(result.Value);
    }

    public static async Task ResultOnSuccessAsync()
    {
        Result result = Result.Success();

        Result afterChain = await result.OnSuccessAsync(
            async () =>
            {
                await Task.Delay(10);

                Console.WriteLine(
                    "Operation completed successfully.");
            });

        Console.WriteLine(afterChain.IsSuccess);
    }

    public static async Task ResultOfTOnSuccessAsync()
    {
        Result<int> result = Result<int>.Success(42);

        Result<int> afterChain = await result.OnSuccessAsync(
            async value =>
            {
                await Task.Delay(10);

                Console.WriteLine(
                    $"Value: {value}");
            });

        Console.WriteLine(afterChain.Value);
    }
}