using System.Net.Http.Json;
using System.Text.Json;
using ApiContracts;

namespace FrontEnd.HttpServices;

public class HttpPostService : IPostService
{
    private readonly HttpClient client;

    public HttpPostService(HttpClient client)
    {
        this.client = client;
    }

    public async Task<PostDto> AddPostAsync(CreatePostDto request)
    {
        HttpResponseMessage response =
            await client.PostAsJsonAsync("Posts", request);

        string content = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
            throw new Exception(content);

        return JsonSerializer.Deserialize<PostDto>(
            content,
            new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            })!;
    }

    public async Task<PostDto> GetPostAsync(int id)
    {
        return await client.GetFromJsonAsync<PostDto>($"Posts/{id}")
               ?? throw new Exception("Post was not found.");
    }

    public async Task<List<PostDto>> GetPostsAsync()
    {
        return await client.GetFromJsonAsync<List<PostDto>>("Posts")
               ?? new List<PostDto>();
    }
}