using Application.Interfaces.DTOs;

namespace Application.DTOs.User;

public class ReadUserPublicDto:IReadUserDto
{
    public required string Username { get; set; }
    public required string Email { get; set; }
    public required DateTime DateOfBirth { get; set; }
}