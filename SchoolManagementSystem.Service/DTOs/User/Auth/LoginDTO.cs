namespace SchoolManagementSystem.Service.DTOs.User.Auth;

public class LoginDTO
{
    public string Identifier { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}