using System.Net.Http.Json;
using System.Text.Json;
using ApiContracts;

namespace FrontEnd.HttpServices;

public class HttpUserService : IUserService
{
    private readonly HttpClient client;

    public HttpUserService(HttpClient client)
    {
        this.client = client;
    }

    public async Task<UserDto> AddUserAsync(CreateUserDto request)
    {
        HttpResponseMessage response =
            await client.PostAsJsonAsync("Users", request);

        string content = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
            throw new Exception(content);

        return JsonSerializer.Deserialize<UserDto>(
            content,
            new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            })!;
    }

    public async Task<UserDto> GetUserAsync(int id)
    {
        return await client.GetFromJsonAsync<UserDto>($"Users/{id}")
               ?? throw new Exception("User was not found.");
    }

    public async Task<List<UserDto>> GetUsersAsync()
    {
        return await client.GetFromJsonAsync<List<UserDto>>("Users")
               ?? new List<UserDto>();
    }
}