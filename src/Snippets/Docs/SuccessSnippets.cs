using Resultron;

namespace Snippets.Docs;

public static class SuccessSnippets
{
    public static void Create()
    {
        var success = new Success(
            "User created successfully.");

        Console.WriteLine(success.Message);
    }

    public static void None()
    {
        Success success = Success.None;

        Console.WriteLine(success.Message);
    }

    public static void WithMetadata()
    {
        var success = new Success(
                "User created successfully.")
            .WithMetadata("UserId", 42)
            .WithMetadata("Operation", "CreateUser");

        Console.WriteLine(success.Metadata["UserId"]);
        Console.WriteLine(success.Metadata["Operation"]);
    }

    public static void ChainMetadata()
    {
        Success success = new Success(
                "Payment completed successfully.")
            .WithMetadata("PaymentId", 12345)
            .WithMetadata("Amount", 250.00m)
            .WithMetadata("Currency", "USD");

        Console.WriteLine(success.Message);
    }
}