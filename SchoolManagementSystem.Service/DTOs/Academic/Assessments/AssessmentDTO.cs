namespace SchoolManagementSystem.Service.DTOs.Academic.Assessments;

public class AssessmentDTO
{
    public decimal NewGradeValue { get; set; }
    public int SubmissionId { get; set; }
    public string? VerbalAssessment { get; set; } 
}