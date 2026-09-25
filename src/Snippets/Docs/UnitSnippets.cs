using Resultron;

namespace Snippets.Docs;

public static class UnitSnippets
{
    public static void Create()
    {
        Unit unit = Unit.Value;

        Console.WriteLine(unit);
    }

    public static void Equality()
    {
        Unit first = Unit.Value;

        Unit second = Unit.Value;

        bool areEqual = first == second;

        Console.WriteLine(areEqual);
    }

    public static void AsResultValue()
    {
        Result<Unit> result = Result<Unit>.Success(Unit.Value);

        Console.WriteLine(result.IsSuccess);
    }
}