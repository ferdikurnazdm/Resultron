using Resultron.Sample.Models;

namespace Resultron.Sample.Repositories;

public interface IUserRepository
{
    Result<User> GetById(Guid id);
    Result<List<User>> GetAll();
    Result<User> Add(User user);
    Result<User> Update(User user);
    Result Delete(Guid id);
    bool EmailExists(string email);
}