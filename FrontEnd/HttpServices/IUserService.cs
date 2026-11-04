using ApiContracts;

namespace FrontEnd.HttpServices;

public interface IUserService
{
    Task<UserDto> AddUserAsync(CreateUserDto request);
    Task<UserDto> GetUserAsync(int id);
    Task<List<UserDto>> GetUsersAsync();
}