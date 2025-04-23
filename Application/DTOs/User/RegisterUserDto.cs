using System.ComponentModel.DataAnnotations;

namespace Application.DTOs.User;

public class RegisterUserDto
{
    public required string Username { get; set; }
    public required string Email { get; set; }
    [RegularExpression(@"^(?=.*[A-Z])(?=.*[a-z])(?=.*\d)(?=.*[!@#$%^&*()_+\-=\[\]{}|;:,.<>?])[^\s]{8,128}$",
        ErrorMessage = "Password must be 8-128 characters long, contain at least one uppercase letter," +
                       " one lowercase letter, one digit, one special character, and no spaces.")]
    public required string Password { get; set; }
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public required DateTime DateOfBirth { get; set; }
}