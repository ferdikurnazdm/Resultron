using Resultron;

namespace Snippets.Docs;

public static class BaseResultSnippets
{
    public static void CheckStatus(BaseResult result)
    {
        if (result.IsSuccess)
        {
            Console.WriteLine("Operation succeeded.");
        }
        else
        {
            Console.WriteLine(
                $"Operation failed: {result.Error.Description}");
        }
    }

    public static void AccessPrimaryError(BaseResult result)
    {
        if (result.IsFailure)
        {
            Error error = result.Error;

            Console.WriteLine(error.Code);
            Console.WriteLine(error.Description);
        }
    }

    public static void AccessReasons(BaseResult result)
    {
        foreach (IReason reason in result.Reasons)
        {
            Console.WriteLine(reason.Message);
        }
    }

    public static void AccessErrors(BaseResult result)
    {
        foreach (Error error in result.Errors)
        {
            Console.WriteLine(
                $"{error.Code}: {error.Description}");
        }
    }

    public static void AccessSuccesses(BaseResult result)
    {
        foreach (Success success in result.Successes)
        {
            Console.WriteLine(success.Message);
        }
    }
}