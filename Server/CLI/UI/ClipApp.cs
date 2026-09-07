using RepositoryContracts;

namespace CLI.UI;

public class CliApp
{
    private readonly UserView userView;
    private readonly PostView postView;
    private readonly CommentView commentView;

    public CliApp(
        IUserRepository userRepository,
        IPostRepository postRepository,
        ICommentRepository commentRepository)
    {
        userView = new UserView(userRepository);

        postView = new PostView(
            postRepository,
            userRepository,
            commentRepository);

        commentView = new CommentView(
            commentRepository,
            userRepository,
            postRepository);
    }

    public async Task RunAsync()
    {
        bool running = true;

        while (running)
        {
            Console.Clear();
            
            Console.WriteLine();
            Console.WriteLine("1. Create user");
            Console.WriteLine("2. Create post");
            Console.WriteLine("3. Add comment");
            Console.WriteLine("4. View posts");
            Console.WriteLine("5. View specific post");
            Console.WriteLine("0. Exit");
            Console.WriteLine();

            Console.Write("Choose an option: ");
            string? choice = Console.ReadLine();

            Console.Clear();

            switch (choice)
            {
                case "1":
                    await userView.CreateUserAsync();
                    break;

                case "2":
                    await postView.CreatePostAsync();
                    break;

                case "3":
                    await commentView.AddCommentAsync();
                    break;

                case "4":
                    await postView.ViewPostsAsync();
                    break;

                case "5":
                    await postView.ViewSpecificPostAsync();
                    break;

                case "0":
                    running = false;
                    break;

                default:
                    Console.WriteLine("Invalid option.");
                    break;
            }

            if (running)
            {
                Console.WriteLine();
                Console.WriteLine("Press ENTER to continue...");
                Console.ReadLine();
            }
        }
    }
}