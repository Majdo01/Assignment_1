using System.Text.Json;
using Entities;
using RepositoryContracts;

namespace FileRepositories;

public class CommentFileRepository : ICommentRepository
{
    private readonly string filePath = "comments.json";

    public CommentFileRepository()
    {
        if (!File.Exists(filePath))
        {
            File.WriteAllText(filePath, "[]");
        }
    }

    public async Task<Comment> AddAsync(Comment comment)
    {
        string commentsAsJson = await File.ReadAllTextAsync(filePath);

        List<Comment> comments =
            JsonSerializer.Deserialize<List<Comment>>(commentsAsJson)!;

        int maxId = comments.Count > 0 ? comments.Max(c => c.Id) : 0;

        comment.Id = maxId + 1;

        comments.Add(comment);

        commentsAsJson = JsonSerializer.Serialize(comments);

        await File.WriteAllTextAsync(filePath, commentsAsJson);

        return comment;
    }

    public async Task UpdateAsync(Comment comment)
    {
        string commentsAsJson = await File.ReadAllTextAsync(filePath);

        List<Comment> comments =
            JsonSerializer.Deserialize<List<Comment>>(commentsAsJson)!;

        Comment? existingComment =
            comments.SingleOrDefault(c => c.Id == comment.Id);

        if (existingComment == null)
        {
            throw new Exception("Comment not found.");
        }

        existingComment.Body = comment.Body;
        existingComment.PostId = comment.PostId;
        existingComment.UserId = comment.UserId;

        commentsAsJson = JsonSerializer.Serialize(comments);

        await File.WriteAllTextAsync(filePath, commentsAsJson);
    }

    public async Task DeleteAsync(int id)
    {
        string commentsAsJson = await File.ReadAllTextAsync(filePath);

        List<Comment> comments =
            JsonSerializer.Deserialize<List<Comment>>(commentsAsJson)!;

        Comment? comment =
            comments.SingleOrDefault(c => c.Id == id);

        if (comment == null)
        {
            throw new Exception("Comment not found.");
        }

        comments.Remove(comment);

        commentsAsJson = JsonSerializer.Serialize(comments);

        await File.WriteAllTextAsync(filePath, commentsAsJson);
    }

    public async Task<Comment> GetSingleAsync(int id)
    {
        string commentsAsJson = await File.ReadAllTextAsync(filePath);

        List<Comment> comments =
            JsonSerializer.Deserialize<List<Comment>>(commentsAsJson)!;

        Comment? comment =
            comments.SingleOrDefault(c => c.Id == id);

        if (comment == null)
        {
            throw new Exception("Comment not found.");
        }

        return comment;
    }

    public IQueryable<Comment> GetMany()
    {
        string commentsAsJson = File.ReadAllText(filePath);

        List<Comment> comments =
            JsonSerializer.Deserialize<List<Comment>>(commentsAsJson)!;

        return comments.AsQueryable();
    }
}