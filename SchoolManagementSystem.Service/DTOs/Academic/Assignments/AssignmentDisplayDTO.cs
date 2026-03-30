using SchoolManagementSystem.Data.Config;

namespace SchoolManagementSystem.Service.DTOs.Academic.Assignments;

public class AssignmentDisplayDTO
{
    public int Id { get; set; }
    public string AssignmentName { get; set; } = string.Empty;
    public string AssignmentTypeName { get; set; } = string.Empty;
    public SchoolEnums.AssignmentStatus AssignmentStatus { get; set; }
    public string Description { get; set; } = string.Empty;

    public DateTime UploadedAt { get; set; }
    public DateTime DueDate { get; set; }
    
    public bool IsOverdue => AssignmentStatus == SchoolEnums.AssignmentStatus.Pending && DueDate < DateTime.Now;
}