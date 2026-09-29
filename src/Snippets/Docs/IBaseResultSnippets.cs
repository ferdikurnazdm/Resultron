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
            Console.WriteLine(
                $"Operation failed: {result.Error.Description}");
        }
    }

    public static void ReadError(IBaseResult result)
    {
        if (result.IsFailure)
        {
            Error error = result.Error;

            Console.WriteLine(error.Code);
            Console.WriteLine(error.Description);
        }
    }
}