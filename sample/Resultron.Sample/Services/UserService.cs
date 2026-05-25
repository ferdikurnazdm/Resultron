using Resultron.Sample.Errors;
using Resultron.Sample.Models;
using Resultron.Sample.Repositories;

namespace Resultron.Sample.Services;

public sealed class UserService
{
    private readonly IUserRepository _repository;

    public UserService(IUserRepository repository) => _repository = repository;

    // Validation → Email check → Create → zincir örneği
    public Result<User> Create(string name, string email)
    {
        return ValidateName(name)
            .Bind(() => ValidateEmail(email))
            .Bind(() => CheckEmailUniqueness(email))
            .Bind(() => SaveUser(name, email));
    }

    public Result<User> GetById(Guid id) => _repository.GetById(id);

    public Result<List<User>> GetAll() => _repository.GetAll();

    // Map örneği — User'ı güncelleyip geri döndürüyor
    public Result<User> UpdateName(Guid id, string newName)
    {
        return ValidateName(newName)
            .Bind(() => _repository.GetById(id))
            .Map(user => new User(user.Id, newName, user.Email, user.CreatedAt))
            .Bind(_repository.Update);
    }

    public Result Delete(Guid id)
    {
        return _repository.GetById(id)
            .Bind(user => _repository.Delete(user.Id));
    }

    // --- Private helpers ---

    private static Result ValidateName(string name)
    {
        return name.Length >= 2
            ? Result.Success()
            : Result.Failure(UserErrors.InvalidName);
    }

    private static Result ValidateEmail(string email)
    {
        return email.Contains('@')
            ? Result.Success()
            : Result.Failure(UserErrors.InvalidEmail);
    }

    private Result CheckEmailUniqueness(string email)
    {
        return _repository.EmailExists(email)
            ? Result.Failure(UserErrors.EmailAlreadyExists(email))
            : Result.Success();
    }

    private Result<User> SaveUser(string name, string email)
    {
        var user = new User(Guid.NewGuid(), name, email, DateTime.UtcNow);
        return _repository.Add(user);
    }
}
