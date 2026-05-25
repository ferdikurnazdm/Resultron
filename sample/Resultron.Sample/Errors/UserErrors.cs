namespace Resultron.Sample.Errors;

public static class UserErrors
{
    public static Error NotFound(Guid id) =>
        new("User.NotFound", $"User with id '{id}' was not found.");

    public static Error EmailAlreadyExists(string email) =>
        new("User.EmailAlreadyExists", $"A user with email '{email}' already exists.");

    public static Error InvalidName =>
        new("User.InvalidName", "Name must be at least 2 characters.");

    public static Error InvalidEmail =>
        new("User.InvalidEmail", "Email must contain '@'.");
}
