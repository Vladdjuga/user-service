namespace Application.Auth;

public interface IJwtProvider
{
    public string GenerateToken(Guid userSubject, string userName,string userEmail);
}