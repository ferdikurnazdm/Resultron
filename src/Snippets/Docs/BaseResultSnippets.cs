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

    public static void AccessError(BaseResult result)
    {
        if (result.IsFailure)
        {
            Error error = result.Error;

            Console.WriteLine(error.Code);
            Console.WriteLine(error.Description);
        }
    }
}
