namespace SchoolManagementSystem.Service.DTOs.User.Display;

public class UserDisplayDto
{
    public string RoleName { get; set; } = string.Empty;
    
    public string FullName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string? PrivateId { get; set; } = string.Empty;
    public DateTime? DateOfBirth { get; set; }
    public DateTime? RegisterDate { get; set; }

    public string? GroupName { get; set; } = string.Empty;
    public string? OfficeRoomName { get; set; } = string.Empty;
    public decimal? FinalGrade { get; set; } 
}