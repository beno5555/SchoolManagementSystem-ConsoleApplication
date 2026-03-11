using System.ComponentModel.DataAnnotations;
using SchoolManagementSystem.Data.Constants;
using SchoolManagementSystem.Data.Models.UserProfiles;

namespace SchoolManagementSystem.Data.Models;

// a single recorded assessment
public class Assessment
{
    public int GradeId { get; set; }
    [MinLength(0), MaxLength(10)]
    public decimal GradeValue { get; set; }
    public DateTime DateAssessed { get; set; } = DateTime.Now;
    public required TeacherProfile AssessedBy { get; set; }
    public required StudentProfile AssessedTo { get; set; }
    public required Subject SubjectAssessedIn { get; set; }
    public Assignment? AssignmentAssessedFor { get; set; }
    public Enums.AssessmentType AssessmentType { get; set; }
    [MaxLength(500)]
    public string? Comment { get; set; }
}