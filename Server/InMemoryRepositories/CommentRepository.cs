using Entities;
using RepositoryContracts;

namespace InMemoryRepositories;

public class CommentRepository : ICommentRepository
{
    private readonly List<Comment> comments = [];

    public CommentRepository()
    {
        _ = AddAsync(new Comment("I definitely prefer gaming on PC!", 1, 1)).Result;
        _ = AddAsync(new Comment("Same here, PC is the best!", 1, 2)).Result;
        _ = AddAsync(new Comment("I mostly play multiplayer games.", 1, 2)).Result;
        _ = AddAsync(new Comment("I'm more into story games.", 1, 1)).Result;
        _ = AddAsync(new Comment("Any recommendations?", 1, 4)).Result;

        _ = AddAsync(new Comment("My weekend is looking pretty chill.", 2, 2)).Result;
        _ = AddAsync(new Comment("I'm probably staying home and gaming :D", 2, 3)).Result;
        _ = AddAsync(new Comment("Sounds like a good plan!", 2, 2)).Result;
        _ = AddAsync(new Comment("I might go out with some friends.", 2, 1)).Result;

        _ = AddAsync(new Comment("#FIRST", 3, 1)).Result;
        _ = AddAsync(new Comment("Coffee in the morning, always.", 3, 2)).Result;
        _ = AddAsync(new Comment("Tea is much better!", 3, 4)).Result;
        _ = AddAsync(new Comment("Depends on the day honestly.", 3, 4)).Result;

        _ = AddAsync(new Comment("I'd recommend watching something funny.", 4, 4)).Result;
        _ = AddAsync(new Comment("I'm more into action movies.", 4, 3)).Result;
        _ = AddAsync(new Comment("A good comedy never disappoints.", 4, 4)).Result;
        _ = AddAsync(new Comment("What about horror movies?", 4, 1)).Result;

        _ = AddAsync(new Comment("I really want to improve my programming skills.", 5, 1)).Result;
        _ = AddAsync(new Comment("C# is giving me a headache sometimes.", 5, 1)).Result;
        _ = AddAsync(new Comment("Practice makes perfect!", 5, 2)).Result;
        _ = AddAsync(new Comment("You could try building a small project.", 5, 3)).Result;

        _ = AddAsync(new Comment("Mine has way too many cables!", 6, 2)).Result;
        _ = AddAsync(new Comment("A good monitor makes such a difference.", 6, 4)).Result;
        _ = AddAsync(new Comment("I definitely need a bigger desk.", 6, 3)).Result;
        _ = AddAsync(new Comment("RGB makes everything faster, obviously.", 6, 1)).Result;
    }

    public Task<Comment> AddAsync(Comment comment)
    {
        comment.Id = comments.Any()
            ? comments.Max(c => c.Id) + 1
            : 1;

        comments.Add(comment);

        return Task.FromResult(comment);
    }

    public Task UpdateAsync(Comment comment)
    {
        Comment? existingComment =
            comments.SingleOrDefault(c => c.Id == comment.Id);

        if (existingComment is null)
        {
            throw new InvalidOperationException(
                $"Comment with ID '{comment.Id}' not found");
        }

        comments.Remove(existingComment);
        comments.Add(comment);

        return Task.CompletedTask;
    }

    public Task DeleteAsync(int id)
    {
        Comment? commentToRemove =
            comments.SingleOrDefault(c => c.Id == id);

        if (commentToRemove is null)
        {
            throw new InvalidOperationException(
                $"Comment with ID '{id}' not found");
        }

        comments.Remove(commentToRemove);

        return Task.CompletedTask;
    }

    public Task<Comment> GetSingleAsync(int id)
    {
        Comment? comment =
            comments.SingleOrDefault(c => c.Id == id);

        if (comment is null)
        {
            throw new InvalidOperationException(
                $"Comment with ID '{id}' not found");
        }

        return Task.FromResult(comment);
    }

    public IQueryable<Comment> GetMany()
    {
        return comments.AsQueryable();
    }
}