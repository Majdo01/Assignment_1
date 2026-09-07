using CLI.UI;
using InMemoryRepositories;
using RepositoryContracts;

IUserRepository userRepository = new UserRepository();
IPostRepository postRepository = new PostRepository();
ICommentRepository commentRepository = new CommentRepository();

CliApp cliApp = new CliApp(
    userRepository,
    postRepository,
    commentRepository);

await cliApp.RunAsync();