using Entities;
using RepositoryContracts;

namespace CLI.UI;

public class PostView
{
    private readonly IPostRepository postRepository;
    private readonly IUserRepository userRepository;
    private readonly ICommentRepository commentRepository;

    public PostView(
        IPostRepository postRepository,
        IUserRepository userRepository,
        ICommentRepository commentRepository)
    {
        this.postRepository = postRepository;
        this.userRepository = userRepository;
        this.commentRepository = commentRepository;
    }

    public async Task CreatePostAsync()
    {
        Console.WriteLine("=== Create Post ===");
        Console.WriteLine();

        Console.Write("Title: ");
        string? title = Console.ReadLine();

        if (string.IsNullOrWhiteSpace(title))
        {
            Console.WriteLine("Title cannot be empty.");
            return;
        }

        Console.Write("Body: ");
        string? body = Console.ReadLine();

        if (string.IsNullOrWhiteSpace(body))
        {
            Console.WriteLine("Body cannot be empty.");
            return;
        }

        Console.Write("User ID: ");
        string? userIdInput = Console.ReadLine();

        if (!int.TryParse(userIdInput, out int userId))
        {
            Console.WriteLine("Invalid user ID.");
            return;
        }

        User? user = await userRepository.GetSingleAsync(userId);

        if (user == null)
        {
            Console.WriteLine("User does not exist.");
            return;
        }

        Post post = new Post(title, body, userId);

        await postRepository.AddAsync(post);

        Console.WriteLine();
        Console.WriteLine($"Post created successfully. ID: {post.Id}");
    }

    public async Task ViewPostsAsync()
    {
        Console.WriteLine("=== Posts ===");
        Console.WriteLine();

        List<Post> posts = postRepository.GetMany().ToList();

        if (!posts.Any())
        {
            Console.WriteLine("No posts found.");
            return;
        }

        foreach (Post post in posts)
        {
            Console.WriteLine($"[{post.Title}, {post.Id}]");
        }
    }

    public async Task ViewSpecificPostAsync()
    {
        Console.WriteLine("=== View Post ===");
        Console.WriteLine();

        Console.Write("Post ID: ");
        string? postIdInput = Console.ReadLine();

        if (!int.TryParse(postIdInput, out int postId))
        {
            Console.WriteLine("Invalid post ID.");
            return;
        }

        Post? post = await postRepository.GetSingleAsync(postId);

        if (post == null)
        {
            Console.WriteLine("Post does not exist.");
            return;
        }

        Console.WriteLine();
        Console.WriteLine($"Title: {post.Title}");
        Console.WriteLine($"Body: {post.Body}");
        Console.WriteLine($"User ID: {post.UserId}");
        Console.WriteLine();

        Console.WriteLine("Comments:");

        List<Comment> comments = commentRepository
            .GetMany()
            .Where(c => c.PostId == post.Id)
            .ToList();

        if (!comments.Any())
        {
            Console.WriteLine("No comments.");
            return;
        }

        foreach (Comment comment in comments)
        {
            Console.WriteLine($"- {comment.Body} (User ID: {comment.UserId})");
        }
    }
}