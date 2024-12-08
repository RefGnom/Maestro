using Maestro.Data.Models;

namespace Maestro.Server.Repositories;

public interface IUsersRepository
{
    List<User> Get();
    void Add();
}