using Resultron;

namespace Snippets.Docs;

public static class ErrorSnippets
{
    public static void Create()
    {
        var error = new Error(
            "user.not_found",
            "The requested user was not found.");

        Console.WriteLine(error.Code);
        Console.WriteLine(error.Description);
    }

    public static void None()
    {
        Error error = Error.None;

        Console.WriteLine(error.Code);
        Console.WriteLine(error.Description);
    }

    public static void CreateWithException()
    {
        var exception = new InvalidOperationException(
            "Database connection failed.");

        var error = new Error(
            "database.connection_failed",
            "Unable to connect to the database.",
            exception);

        Console.WriteLine(error.Exception?.Message);
    }

    public static void WithMetadata()
    {
        var error = new Error(
                "user.not_found",
                "The requested user was not found.")
            .WithMetadata("UserId", 42)
            .WithMetadata("Source", "Database");

        Console.WriteLine(error.Metadata["UserId"]);
        Console.WriteLine(error.Metadata["Source"]);
    }

    public static void CausedBy()
    {
        var exception = new InvalidOperationException(
            "Database connection failed.");

        var error = new Error(
                "database.error",
                "An error occurred while accessing the database.")
            .CausedBy(exception);

        Console.WriteLine(error.Exception?.Message);
    }

    public static void CombineMetadataAndException()
    {
        var exception = new InvalidOperationException(
            "Database connection failed.");

        var error = new Error(
                "database.error",
                "An error occurred while accessing the database.")
            .WithMetadata("Operation", "GetUser")
            .WithMetadata("UserId", 42)
            .CausedBy(exception);

        Console.WriteLine(error.Code);
    }
}