using Application.Interfaces.Security;
using System.Security.Cryptography;

namespace Application.Services.Security;

public class Pbkdf2PasswordHasher:IPasswordHasher
{
    public string HashPassword(string password)
    {
        var salt = RandomNumberGenerator.GetBytes(18);
        var hash = Rfc2898DeriveBytes.Pbkdf2(
            password,
            salt,
            100,
            HashAlgorithmName.SHA256, 
            32);
        return Convert.ToBase64String(salt)+Convert.ToBase64String(hash);
    }

    public bool VerifyPassword(string password, string hashedPassword)
    {
        var salt = Convert.FromBase64String(hashedPassword.Substring(0,24));
        var hash = Convert.FromBase64String(hashedPassword.Substring(24));
        var newHash = Rfc2898DeriveBytes.Pbkdf2(
            password,
            salt,
            100,
            HashAlgorithmName.SHA256, 
            32);
        return hash.SequenceEqual(newHash);
    }
}