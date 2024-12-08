using Maestro.Core.Models;
using Maestro.Server.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Maestro.Server.Controllers;

[ApiController]
[Route("users")]
public class UsersController(IUsersRepository usersRepository) : ControllerBase
{
    private readonly IUsersRepository _usersRepository = usersRepository;
    
    [HttpGet("get")]
    public IActionResult Get()
    {
        return new ObjectResult(new
        {
            Users = _usersRepository.Get().Select(user => new User { Id = user.Id })
        });
    }

    [HttpGet("add")]
    public IActionResult Add()
    {
        _usersRepository.Add();

        return new OkResult();
    }
}