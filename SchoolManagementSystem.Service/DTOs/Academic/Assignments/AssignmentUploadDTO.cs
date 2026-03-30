using SchoolManagementSystem.Data.Config;

namespace SchoolManagementSystem.Service.DTOs.Academic.Assignments;

public class AssignmentUploadDTO
{
    public string AssignmentName { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public int AssignmentTypeId { get; set; }
    public int GroupId { get; set; }
    public int SubjectId { get; set; }
    public DateTime DueDate { get; set; }
}