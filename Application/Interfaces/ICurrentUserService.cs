namespace Application.Interfaces;

public interface ICurrentUserService
{
    Guid UserId { get; }
    string? Username { get; }
    string? Email { get; }
}