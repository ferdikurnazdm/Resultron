using Resultron;

namespace Snippets.Docs;

public static class IReasonSnippets
{
    public static void AccessReason()
    {
        IReason reason = new Success(
            "Operation completed successfully.");

        Console.WriteLine(reason.Message);
        Console.WriteLine(reason.Metadata.Count);
    }

    public static void ErrorAsReason()
    {
        IReason reason = new Error(
            "user.not_found",
            "The requested user was not found.");

        Console.WriteLine(reason.Message);
    }

    public static void SuccessAsReason()
    {
        IReason reason = new Success(
            "User created successfully.");

        Console.WriteLine(reason.Message);
    }

    public static void ReadMetadata()
    {
        IReason reason = new Error(
                "user.not_found",
                "The requested user was not found.")
            .WithMetadata("UserId", 42);

        if (reason.Metadata.TryGetValue("UserId", out object? userId))
        {
            Console.WriteLine(userId);
        }
    }

    public static void HandleReason(IReason reason)
    {
        Console.WriteLine(reason.Message);

        foreach (var metadata in reason.Metadata)
        {
            Console.WriteLine(
                $"{metadata.Key}: {metadata.Value}");
        }
    }
}