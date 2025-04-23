namespace Application.DTOs.User;

public class LoginUserDto
{
    public required string Identity { get; set; }
    public required string Password { get; set; }
}