namespace SchoolManagementSystem.Service.DTOs.Academic.Submissions;

public class SubmissionDTO
{
    public string SubmissionContent { get; set; } = string.Empty;
    public string StudentComment { get; set; } = string.Empty;

    public int AssignmentId { get; set; }
}