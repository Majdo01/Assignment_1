using Entities;
using RepositoryContracts;

namespace CLI.UI;

public class UserView
{
    private readonly IUserRepository userRepository;

    public UserView(IUserRepository userRepository)
    {
        this.userRepository = userRepository;
    }

    public async Task CreateUserAsync()
    {
        Console.WriteLine("=== Create User ===");
        Console.WriteLine();

        Console.Write("Username: ");
        string? username = Console.ReadLine();

        if (string.IsNullOrWhiteSpace(username))
        {
            Console.WriteLine("Username cannot be empty.");
            return;
        }

        bool usernameExists = userRepository
            .GetMany()
            .Any(u => u.UserName.Equals(username, StringComparison.OrdinalIgnoreCase));

        if (usernameExists)
        {
            Console.WriteLine("Username already exists.");
            return;
        }

        Console.Write("Password: ");
        string? password = Console.ReadLine();

        if (string.IsNullOrWhiteSpace(password))
        {
            Console.WriteLine("Password cannot be empty.");
            return;
        }

        User user = new User(username, password);

        await userRepository.AddAsync(user);

        Console.WriteLine();
        Console.WriteLine($"User created successfully. ID: {user.Id}");
    }
}