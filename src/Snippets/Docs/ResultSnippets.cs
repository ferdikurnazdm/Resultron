using Resultron;

namespace Snippets.Docs;

public static class ResultSnippets
{
    public static void Success()
    {
        Result result = Result.Success();

        Console.WriteLine(result.IsSuccess);
    }

    public static void SuccessWithReason()
    {
        var success = new Success(
            "Operation completed successfully.");

        Result result = Result.Success(success);

        Console.WriteLine(result.Successes[0].Message);
    }

    public static void SuccessWithReasons()
    {
        IReason[] reasons =
        [
            new Success("Operation completed successfully.")
                .WithMetadata("Operation", "CreateUser"),

            new Success("Notification sent successfully.")
        ];

        Result result = Result.Success(reasons);

        foreach (IReason reason in result.Reasons)
        {
            Console.WriteLine(reason.Message);
        }
    }

    public static void Failure()
    {
        var error = new Error(
            "user.not_found",
            "The requested user was not found.");

        Result result = Result.Failure(error);

        Console.WriteLine(result.Error.Description);
    }

    public static void FailureWithErrors()
    {
        Error[] errors =
        [
            new(
                "user.not_found",
                "The requested user was not found."),

            new(
                "user.inactive",
                "The user account is inactive.")
        ];

        Result result = Result.Failure(errors);

        foreach (Error error in result.Errors)
        {
            Console.WriteLine(
                $"{error.Code}: {error.Description}");
        }
    }

    public static void FailureWithReasons()
    {
        IReason[] reasons =
        [
            new Error(
                "operation.failed",
                "The operation could not be completed.")
                .WithMetadata("Operation", "CreateUser")
        ];

        Result result = Result.Failure(reasons);

        foreach (IReason reason in result.Reasons)
        {
            Console.WriteLine(reason.Message);
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
            () => Console.WriteLine("başarılı"));

        Console.WriteLine(afterChain.IsSuccess);
    }

    public static void OnFailure()
    {
        Result result = Result.Failure(
            new Error(
                "operation.failed",
                "İşlem başarısız."));

        Result afterChain = result.OnFailure(
            error => Console.WriteLine(error.Description));

        Console.WriteLine(afterChain.IsFailure);
    }
}