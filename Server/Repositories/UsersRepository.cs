using Maestro.Data;
using Maestro.Data.Models;

namespace Maestro.Server.Repositories;

public class UsersRepository(DataContext dataContext) : IUsersRepository
{
    private readonly DataContext _dataContext = dataContext;

    public List<User> Get()
    {
        return _dataContext.Users.ToList();
    }

    public void Add()
    {
        _dataContext.Users.Add(new User());
        _dataContext.SaveChanges();
    }
}