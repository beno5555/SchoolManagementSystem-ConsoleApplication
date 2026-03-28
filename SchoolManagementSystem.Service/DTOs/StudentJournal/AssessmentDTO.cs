namespace SchoolManagementSystem.Service.DTOs.StudentJournal;

public class AssessmentDTO
{
    public decimal NewGradeValue { get; set; }
    public int SubmissionId { get; set; }
    public string? VerbalAssessment { get; set; } 
}