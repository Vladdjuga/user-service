using System.ComponentModel.DataAnnotations;

namespace Application.DTOs.User;

public class UpdateUserDto
{
    public required Guid Id { get; set; }
    public string? Username { get; set; }
    public string? Email { get; set; }
    [RegularExpression(@"^(?=.*[A-Z])(?=.*[a-z])(?=.*\d)(?=.*[!@#$%^&*()_+\-=\[\]{}|;:,.<>?])[^\s]{8,128}$",
        ErrorMessage = "Password must be 8-128 characters long, contain at least one uppercase letter," +
                       " one lowercase letter, one digit, one special character, and no spaces.")]
    public string? Password { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public DateTime? DateOfBirth { get; set; }
}