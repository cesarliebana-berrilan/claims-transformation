namespace Berrilan.Claims.Core.Domain;

public class User
{
    public Guid Id { get; set; }
    public string Email { get; set; } = null!;
    public Guid CustomerId { get; set; }
    public Role Role { get; set; }
    public License License { get; set; }
    public bool IsRoot { get; set; }
}

public enum Role
{
    Admin,
    User,
    Guest
}

public enum License
{
    Free,
    Enterprise
}