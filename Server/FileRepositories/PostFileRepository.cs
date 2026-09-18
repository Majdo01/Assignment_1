using System.Text.Json;
using Entities;
using RepositoryContracts;

namespace FileRepositories;

public class PostFileRepository : IPostRepository
{
    private readonly string filePath = "posts.json";

    public PostFileRepository()
    {
        if (!File.Exists(filePath))
        {
            File.WriteAllText(filePath, "[]");
        }
    }

    public async Task<Post> AddAsync(Post post)
    {
        string postsAsJson = await File.ReadAllTextAsync(filePath);

        List<Post> posts =
            JsonSerializer.Deserialize<List<Post>>(postsAsJson)!;

        int maxId = posts.Count > 0 ? posts.Max(p => p.Id) : 0;

        post.Id = maxId + 1;

        posts.Add(post);

        postsAsJson = JsonSerializer.Serialize(posts);

        await File.WriteAllTextAsync(filePath, postsAsJson);

        return post;
    }

    public async Task UpdateAsync(Post post)
    {
        string postsAsJson = await File.ReadAllTextAsync(filePath);

        List<Post> posts =
            JsonSerializer.Deserialize<List<Post>>(postsAsJson)!;

        Post? existingPost = posts.SingleOrDefault(p => p.Id == post.Id);

        if (existingPost == null)
        {
            throw new Exception("Post not found.");
        }

        existingPost.Title = post.Title;
        existingPost.Body = post.Body;
        existingPost.UserId = post.UserId;

        postsAsJson = JsonSerializer.Serialize(posts);

        await File.WriteAllTextAsync(filePath, postsAsJson);
    }

    public async Task DeleteAsync(int id)
    {
        string postsAsJson = await File.ReadAllTextAsync(filePath);

        List<Post> posts =
            JsonSerializer.Deserialize<List<Post>>(postsAsJson)!;

        Post? post = posts.SingleOrDefault(p => p.Id == id);

        if (post == null)
        {
            throw new Exception("Post not found.");
        }

        posts.Remove(post);

        postsAsJson = JsonSerializer.Serialize(posts);

        await File.WriteAllTextAsync(filePath, postsAsJson);
    }

    public async Task<Post> GetSingleAsync(int id)
    {
        string postsAsJson = await File.ReadAllTextAsync(filePath);

        List<Post> posts =
            JsonSerializer.Deserialize<List<Post>>(postsAsJson)!;

        Post? post = posts.SingleOrDefault(p => p.Id == id);

        if (post == null)
        {
            throw new Exception("Post not found.");
        }

        return post;
    }

    public IQueryable<Post> GetMany()
    {
        string postsAsJson = File.ReadAllText(filePath);

        List<Post> posts =
            JsonSerializer.Deserialize<List<Post>>(postsAsJson)!;

        return posts.AsQueryable();
    }
}