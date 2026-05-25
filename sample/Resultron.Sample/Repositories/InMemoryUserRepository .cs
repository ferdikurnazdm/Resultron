using Resultron.Sample.Errors;
using Resultron.Sample.Models;

namespace Resultron.Sample.Repositories;

public sealed class InMemoryUserRepository : IUserRepository
{
    private readonly Dictionary<Guid, User> _store = [];

    public Result<User> GetById(Guid id)
    {
        return _store.TryGetValue(id, out var user)
            ? Result<User>.Success(user)
            : Result<User>.Failure(UserErrors.NotFound(id));
    }

    public Result<List<User>> GetAll() => Result<List<User>>.Success(_store.Values.ToList());

    public Result<User> Add(User user)
    {
        _store[user.Id] = user;
        return Result<User>.Success(user);
    }

    public Result<User> Update(User user)
    {
        if (!_store.ContainsKey(user.Id))
            return Result<User>.Failure(UserErrors.NotFound(user.Id));

        _store[user.Id] = user;
        return Result<User>.Success(user);
    }

    public Result Delete(Guid id)
    {
        if (!_store.ContainsKey(id))
            return Result.Failure(UserErrors.NotFound(id));

        _store.Remove(id);
        return Result.Success();
    }

    public bool EmailExists(string email) => _store.Values.Any(u => u.Email == email);
}
