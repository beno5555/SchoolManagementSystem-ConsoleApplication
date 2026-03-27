using System.Security.Cryptography;
using ProjectHelperLibrary.Response;

namespace SchoolManagementSystem.Service.BusinessLogic;

public class PasswordHasher
{
    private const int SaltLength = 32;
    private const int HashLength = 32;
    private const int Iterations = 100_000;
    public (string hash, string salt) HashPassword(string password)
    {
        byte[] salt = RandomNumberGenerator.GetBytes(SaltLength);
        byte[] hash = Rfc2898DeriveBytes.Pbkdf2(
            password,
            salt,
            Iterations,
            HashAlgorithmName.SHA256,
            HashLength);
        return (Convert.ToBase64String(hash), Convert.ToBase64String(salt));
    }

    public BaseResponse VerifyPassword(string password, string storedHash, string storedSalt)
    {
        BaseResponse response = new();
        if (!string.IsNullOrWhiteSpace(password))
        {
            byte[] salt = Convert.FromBase64String(storedSalt);

            byte[] hash = Rfc2898DeriveBytes.Pbkdf2(
                password,
                salt,
                Iterations,
                HashAlgorithmName.SHA256,
                HashLength);

            bool isValid = CryptographicOperations.FixedTimeEquals(hash, Convert.FromBase64String(storedHash));
            response.SetStatus(isValid, "Invalid password");

        }
        else
        {
            response.SetStatus(false, "password field cannot be empty");
        }
        return response;
    }
}