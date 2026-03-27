using SchoolManagementSystem.Service.DTOs.Assignments;

namespace SchoolManagementSystem.Service.DTOs.StudentJournal;

public class SubjectEnrollmentDTO
{
    public int SubjectEnrollmentId { get; set; }
    public string SubjectName { get; set; } = string.Empty;
    public string TeacherName { get; set; } = string.Empty;
    public decimal AverageGrade { get; set; }
    public List<AssessmentDTO>? Assessments { get; set; }
    public List<AssignmentDisplayDTO>? PendingAssignments { get; set; }
}
