namespace TodoList.Domain.Entities;

public class User
{
    public int ID { get; private set; }
    public string FirstName { get; private set; } = string.Empty;
    public string? LastName { get; private set; }
    public string Username { get; private set; } = string.Empty;
    public string Password { get; private set; } = string.Empty;
    public DateTime CreatedAt { get; private set; }
    public DateTime UpdatedAt { get; private set; }
    private User() { }
    public User(string firstName, string? lastName, string username, string password)
    {
        if (string.IsNullOrWhiteSpace(firstName))
            throw new ArgumentException("FirstName cannot be empty.", nameof(firstName));
        if (string.IsNullOrWhiteSpace(username))
            throw new ArgumentException("Username cannot be empty.", nameof(username));
        if (string.IsNullOrWhiteSpace(password))
            throw new ArgumentException("Password cannot be empty.", nameof(password));
        FirstName = firstName;
        LastName = lastName;
        Username = username;
        Password = password;
        CreatedAt = DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;
    }
    public void UpdateFirstName(string firstName)
    {
        if (string.IsNullOrWhiteSpace(firstName))
            throw new ArgumentException("FirstName cannot be empty.", nameof(firstName));
        FirstName = firstName;
        UpdatedAt = DateTime.UtcNow;
    }
    public void UpdateLastName(string? lastName)
    {
        LastName = lastName;
        UpdatedAt = DateTime.UtcNow;
    }
    public void UpdateUsername(string username)
    {
        if (string.IsNullOrWhiteSpace(username))
            throw new ArgumentException("Username cannot be empty.", nameof(username));
        Username = username;
        UpdatedAt = DateTime.UtcNow;
    }
    public void UpdatePassword(string password)
    {
        if (string.IsNullOrWhiteSpace(password))
            throw new ArgumentException("Password cannot be empty.", nameof(password));
        Password = password;
        UpdatedAt = DateTime.UtcNow;
    }
}