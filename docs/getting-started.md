# Getting Started

Follow this guide to install Resultron and implement your first functional result workflow.

## Installation

You can install Resultron via the NuGet Package Manager Console:

```bash
dotnet add package Resultron
```

## Your First Result

Here is a simple example of how to refactor a traditional `try-catch` or `null` return into a clean, explicit **Resultron** workflow.

### 1. Creating a Service

```csharp
using Resultron;

public class UserService
{
    public Result<User> GetUser(int id)
    {
        if (id <= 0)
        {
            // Returns a failed result with an explicit Error object
            return Result<User>.Failure(new Error("Invalid user ID provided."));
        }

        var user = _db.Users.Find(id);
        if (user == null)
        {
            // Returns a failed result indicating the resource was not found
            return Result<User>.Failure(new Error($"User with ID {id} was not found."));
        }

        // Returns a successful result wrapped with the user data
        return Result<User>.Success(user);
    }
}
```


### 2. Consuming the Result in a Controller

You can seamlessly process the outcome inside your ASP.NET Core controllers by evaluating the `IsFailure` property. If the operation fails, the structured `Error` object can be returned directly in the HTTP response.

```csharp
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly UserService _userService;

    public UsersController(UserService userService)
    {
        _userService = userService;
    }

    [HttpGet("{id}")]
    public IActionResult GetUserById(int id)
    {
        var result = _userService.GetUser(id);

        if (result.IsFailure)
        {
            // Returns an HTTP 400 Bad Request with the detailed Error object
            return BadRequest(result.Error);
        }

        // Returns an HTTP 200 OK with the requested user data
        return Ok(result.Value);
    }
}
```

## Operations Without a Return Value (Void Operations)

For actions that perform a task but do not need to return a specific data model (such as updating a database record, deleting an entity, or changing a password), you can use the non-generic Result type.

Example: Deleting a Resource

```csharp
using Resultron;

public class UserManagementService
{
    public Result DeleteUser(int id)
    {
        var user = _db.Users.Find(id);
        if (user == null)
        {
            // Returns a non-generic failure result
            return Result.Failure(new Error("User to delete was not found."));
        }

        _db.Users.Remove(user);
        _db.SaveChanges();

        // Returns a simple success indicator with no value wrapped inside
        return Result.Success();
    }
}
```