namespace SchoolManagementSystem.Service.DTOs.User.Display;

public class UserDisplayDTO
{
    public string RoleName { get; set; } = string.Empty;
    
    public string FullName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string? PrivateId { get; set; }
    public DateTime? DateOfBirth { get; set; }
    public DateTime? RegisterDate { get; set; }

    public string? GroupName { get; set; } 
    public string? OfficeRoomName { get; set; }
    public decimal? FinalGrade { get; set; } 
}