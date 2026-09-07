using Entities;
using RepositoryContracts;

namespace InMemoryRepositories;

public class PostRepository : IPostRepository
{
    private readonly List<Post> posts = [];
    
    public PostRepository()
    {
        _ = AddAsync(new Post("Favorite video game?", "What's everyone's favorite game right now?", 1)).Result;
        _ = AddAsync(new Post("Weekend plans", "Anyone doing anything interesting this weekend?", 1)).Result;
        _ = AddAsync(new Post("Coffee or tea?", "Which one do you prefer in the morning?", 3)).Result;
        _ = AddAsync(new Post("Programming help", "Does anyone know a good way to learn programming?", 2)).Result;
        _ = AddAsync(new Post("Best movie?", "What movie would you recommend watching tonight?", 4)).Result;
        _ = AddAsync(new Post("Gaming setup", "What does your ideal gaming setup look like?", 3)).Result;
    }

    public Task<Post> AddAsync(Post post)
    {
        post.Id = posts.Any()
            ? posts.Max(p => p.Id) + 1
            : 1;

        posts.Add(post);

        return Task.FromResult(post);
    }

    public Task UpdateAsync(Post post)
    {
        Post? existingPost = posts.SingleOrDefault(p => p.Id == post.Id);

        if (existingPost is null)
        {
            throw new InvalidOperationException(
                $"Post with ID '{post.Id}' not found");
        }

        posts.Remove(existingPost);
        posts.Add(post);

        return Task.CompletedTask;
    }

    public Task DeleteAsync(int id)
    {
        Post? postToRemove = posts.SingleOrDefault(p => p.Id == id);

        if (postToRemove is null)
        {
            throw new InvalidOperationException(
                $"Post with ID '{id}' not found");
        }

        posts.Remove(postToRemove);

        return Task.CompletedTask;
    }

    public Task<Post> GetSingleAsync(int id)
    {
        Post? post = posts.SingleOrDefault(p => p.Id == id);

        if (post is null)
        {
            throw new InvalidOperationException(
                $"Post with ID '{id}' not found");
        }

        return Task.FromResult(post);
    }

    public IQueryable<Post> GetMany()
    {
        return posts.AsQueryable();
    }
}