using Entities;
using RepositoryContracts;

namespace CLI.UI;

public class CommentView
{
    private readonly ICommentRepository commentRepository;
    private readonly IUserRepository userRepository;
    private readonly IPostRepository postRepository;

    public CommentView(
        ICommentRepository commentRepository,
        IUserRepository userRepository,
        IPostRepository postRepository)
    {
        this.commentRepository = commentRepository;
        this.userRepository = userRepository;
        this.postRepository = postRepository;
    }

    public async Task AddCommentAsync()
    {
        Console.WriteLine("=== Add Comment ===");
        Console.WriteLine();

        Console.Write("Comment body: ");
        string? body = Console.ReadLine();

        if (string.IsNullOrWhiteSpace(body))
        {
            Console.WriteLine("Comment cannot be empty.");
            return;
        }

        Console.Write("User ID: ");
        string? userIdInput = Console.ReadLine();

        if (!int.TryParse(userIdInput, out int userId))
        {
            Console.WriteLine("Invalid user ID.");
            return;
        }

        try
        {
            await userRepository.GetSingleAsync(userId);
        }
        catch
        {
            Console.WriteLine("User does not exist.");
            return;
        }

        Console.Write("Post ID: ");
        string? postIdInput = Console.ReadLine();

        if (!int.TryParse(postIdInput, out int postId))
        {
            Console.WriteLine("Invalid post ID.");
            return;
        }

        try
        {
            await postRepository.GetSingleAsync(postId);
        }
        catch
        {
            Console.WriteLine("Post does not exist.");
            return;
        }

        Comment comment = new Comment(body, postId, userId);

        await commentRepository.AddAsync(comment);

        Console.WriteLine();
        Console.WriteLine($"Comment added successfully. ID: {comment.Id}");
    }
}