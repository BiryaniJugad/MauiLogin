using System.Collections.Generic;
using System.Linq;

namespace MauiLogin;

public static class UserRepository
{
    private static List<User> _users = new();

    static UserRepository()
    {
        // Default admin account
        _users.Add(new User
        {
            Username = "admin",
            Email = "admin@system.com",
            Password = "admin123",
            Gender = "Other"
        });
    }
    public static bool UserExists(string username, string email)
    {
        return _users.Any(u =>
        u.Username.Equals(username, StringComparison.OrdinalIgnoreCase) ||
        u.Email.Equals(email, StringComparison.OrdinalIgnoreCase));
    }

    public static void AddUser(User user) => _users.Add(user);

    public static User GetUser(string input, string password)
    {
        return _users.FirstOrDefault(u =>
            (u.Username.Equals(input, StringComparison.OrdinalIgnoreCase) ||
             u.Email.Equals(input, StringComparison.OrdinalIgnoreCase))
            && u.Password == password);

    }

    public static List<User> GetAllUsers() => _users;

    public static void UpdateUser(User updatedUser)
    {
        var existing = _users.FirstOrDefault(u => u.Email == updatedUser.Email);
        if (existing != null)
        {
            existing.Username = updatedUser.Username;
            existing.Password = updatedUser.Password;
            existing.Gender = updatedUser.Gender;
        }
    }
}