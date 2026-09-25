using Resultron;

namespace Snippets.Docs;

public static class OnFailureSnippets
{
    public static void ResultOnFailure()
    {
        Result result = Result.Failure(
            new Error(
                "operation.failed",
                "The operation failed."));

        Result afterChain = result.OnFailure(
            error => Console.WriteLine(error.Description));

        Console.WriteLine(afterChain.IsFailure);
    }

    public static void ResultOnFailureIsNotExecutedOnSuccess()
    {
        Result result = Result.Success();

        Result afterChain = result.OnFailure(
            error => Console.WriteLine(error.Description));

        Console.WriteLine(afterChain.IsSuccess);
    }

    public static void ResultOfTOnFailure()
    {
        Result<int> result = Result<int>.Failure(
            new Error(
                "value.invalid",
                "The value is invalid."));

        Result<int> afterChain = result.OnFailure(
            error => Console.WriteLine(error.Description));

        Console.WriteLine(afterChain.IsFailure);
    }

    public static void ResultOfTPreservesType()
    {
        Result<int> result = Result<int>.Failure(
            new Error(
                "value.invalid",
                "The value is invalid."));

        Result<int> afterChain = result.OnFailure(
            error => Console.WriteLine(error.Description));

        Console.WriteLine(afterChain.Error.Code);
    }

    public static void FluentChain()
    {
        Result<int> result = Result<int>
            .Failure(
                new Error(
                    "value.invalid",
                    "The value is invalid."))
            .OnFailure(
                error => Console.WriteLine(error.Description));

        Console.WriteLine(result.IsFailure);
    }

    public static async Task ResultOnFailureAsync()
    {
        Result result = Result.Failure(
            new Error(
                "operation.failed",
                "The operation failed."));

        Result afterChain = await result.OnFailureAsync(
            async error =>
            {
                await Task.Delay(10);

                Console.WriteLine(error.Description);
            });

        Console.WriteLine(afterChain.IsFailure);
    }

    public static async Task ResultOfTOnFailureAsync()
    {
        Result<int> result = Result<int>.Failure(
            new Error(
                "value.invalid",
                "The value is invalid."));

        Result<int> afterChain = await result.OnFailureAsync(
            async error =>
            {
                await Task.Delay(10);

                Console.WriteLine(error.Description);
            });

        Console.WriteLine(afterChain.IsFailure);
    }
}
