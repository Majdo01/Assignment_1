using ApiContracts;

namespace FrontEnd.HttpServices;

public interface IPostService
{
    Task<PostDto> AddPostAsync(CreatePostDto request);
    Task<PostDto> GetPostAsync(int id);
    Task<List<PostDto>> GetPostsAsync();
}