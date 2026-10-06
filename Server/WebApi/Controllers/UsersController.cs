using ApiContracts;
using Entities;
using Microsoft.AspNetCore.Mvc;
using RepositoryContracts;

namespace WebApi.Controllers;

[ApiController]
[Route("[controller]")]
public class UsersController : ControllerBase
{
    private readonly IUserRepository userRepository;

    public UsersController(IUserRepository userRepository)
    {
        this.userRepository = userRepository;
    }

    [HttpPost]
    public async Task<ActionResult<UserDto>> AddUser(
        [FromBody] CreateUserDto request)
    {
        User user = new User(
            request.UserName,
            request.Password);

        User created = await userRepository.AddAsync(user);

        UserDto dto = new UserDto
        {
            Id = created.Id,
            UserName = created.UserName
        };

        return Created($"/Users/{dto.Id}", dto);
    }
    
    [HttpGet("{id:int}")]
    public async Task<ActionResult<UserDto>> GetUser(int id)
    {
        try
        {
            User user = await userRepository.GetSingleAsync(id);

            UserDto dto = new UserDto
            {
                Id = user.Id,
                UserName = user.UserName
            };

            return Ok(dto);
        }
        catch
        {
            return NotFound();
        }
    }
    
    [HttpGet]
    public ActionResult<IEnumerable<UserDto>> GetUsers(
        [FromQuery] string? userName)
    {
        IQueryable<User> users = userRepository.GetMany();

        if (!string.IsNullOrWhiteSpace(userName))
        {
            users = users.Where(u =>
                u.UserName.Contains(userName,
                    StringComparison.OrdinalIgnoreCase));
        }

        List<UserDto> result = users
            .Select(u => new UserDto
            {
                Id = u.Id,
                UserName = u.UserName
            })
            .ToList();

        return Ok(result);
    }
    
    [HttpPut("{id:int}")]
    public async Task<ActionResult> UpdateUser(
        int id,
        [FromBody] CreateUserDto request)
    {
        User user = new User(
            request.UserName,
            request.Password)
        {
            Id = id
        };

        try
        {
            await userRepository.UpdateAsync(user);
            return NoContent();
        }
        catch
        {
            return NotFound();
        }
    }
    
    [HttpDelete("{id:int}")]
    public async Task<ActionResult> DeleteUser(int id)
    {
        try
        {
            await userRepository.DeleteAsync(id);
            return NoContent();
        }
        catch
        {
            return NotFound();
        }
    }
}    