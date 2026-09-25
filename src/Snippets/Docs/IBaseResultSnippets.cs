using Resultron;

namespace Snippets.Docs;

public static class IBaseResultSnippets
{
    public static void CheckStatus(IBaseResult result)
    {
        if (result.IsSuccess)
        {
            Console.WriteLine("Operation succeeded.");
        }

        if (result.IsFailure)
        {
            Console.WriteLine("Operation failed.");
        }
    }

    public static void ReadReasons(IBaseResult result)
    {
        foreach (IReason reason in result.Reasons)
        {
            Console.WriteLine(reason.Message);
        }
    }

    public static void ReadErrors(IBaseResult result)
    {
        foreach (Error error in result.Errors)
        {
            Console.WriteLine(
                $"{error.Code}: {error.Description}");
        }
    }

    public static void ReadSuccesses(IBaseResult result)
    {
        foreach (Success success in result.Successes)
        {
            Console.WriteLine(success.Message);
        }
    }
}