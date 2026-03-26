using SchoolManagementSystem.Data.Models;

namespace SchoolManagementSystem.Service.DTOs.StudentJournal;

public class SubjectEnrollmentDto
{
    public int SubjectEnrollmentId { get; set; }
    public string SubjectName { get; set; } = string.Empty;
    public string TeacherName { get; set; } = string.Empty;
    public decimal AverageGrade { get; set; }
    public List<AssessmentDto>? Assessments { get; set; }
    public List<AssignmentDto>? PendingAssignments { get; set; }
}
