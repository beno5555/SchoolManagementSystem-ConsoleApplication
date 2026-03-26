namespace SchoolManagementSystem.Service.DTOs.User.Auth;

public class LoginDto
{
    public string? Email { get; set; } = string.Empty;
    public string? PrivateId { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}