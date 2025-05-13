using System.ComponentModel.DataAnnotations;

namespace Application.DTOs.User;

public class RegisterUserDto
{
    public required string Username { get; set; }
    public required string Email { get; set; }
    public required string Password { get; set; }
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public required DateTime DateOfBirth { get; set; }
}