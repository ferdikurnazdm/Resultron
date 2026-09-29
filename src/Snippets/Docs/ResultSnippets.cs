using Resultron;

namespace Snippets.Docs;

public static class ResultSnippets
{
    public static void Success()
    {
        Result result = Result.Success();

        Console.WriteLine(result.IsSuccess);
    }

    public static void Failure()
    {
        var error = new Error(
            "user.not_found",
            "The requested user was not found.");

        Result result = Result.Failure(error);

        Console.WriteLine(result.IsFailure);
        Console.WriteLine(result.Error.Code);
        Console.WriteLine(result.Error.Description);
    }

    public static void FailureWithMetadata()
    {
        var error = new Error(
            "operation.failed",
            "The operation could not be completed.")
            .WithMetadata("Operation", "CreateUser");

        Result result = Result.Failure(error);

        Console.WriteLine(result.Error.Code);
        Console.WriteLine(result.Error.Description);

        if (result.Error.Metadata.TryGetValue(
            "Operation",
            out var operation))
        {
            Console.WriteLine(operation);
        }
    }

    public static void ImplicitConversionFromError()
    {
        var error = new Error(
            "user.not_found",
            "The requested user was not found.");

        Result result = error;

        Console.WriteLine(result.IsFailure);
        Console.WriteLine(result.Error.Code);
    }

    public static Result ReturnSuccess()
    {
        return Result.Success();
    }

    public static Result ReturnFailure()
    {
        return new Error(
            "operation.failed",
            "The operation could not be completed.");
    }

    public static void OnSuccess()
    {
        Result result = Result.Success();

        Result afterChain = result.OnSuccess(
            () => Console.WriteLine("Success"));

        Console.WriteLine(afterChain.IsSuccess);
    }

    public static void OnFailure()
    {
        Result result = Result.Failure(
            new Error(
                "operation.failed",
                "The operation failed."));

        Result afterChain = result.OnFailure(
            error => Console.WriteLine(error.Description));

        Console.WriteLine(afterChain.IsFailure);
    }
}