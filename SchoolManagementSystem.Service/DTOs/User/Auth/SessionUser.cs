namespace SchoolManagementSystem.Service.DTOs.User.Auth;

public class SessionUser
{
    public int Id { get; init; }
    public required string FullName { get; init; }
    public required string Email { get; init; }
    public required string Role { get; init; }

}