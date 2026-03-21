using System.ComponentModel.DataAnnotations;
using SchoolManagementSystem.Data.HelperClasses;
using SchoolManagementSystem.Data.Models.Base;

namespace SchoolManagementSystem.Data.Models;

[FileNamePrefix("assessments")]
public class Assessment : BaseModel
{
    [Range(0, 10)]
    public decimal GradeValue { get; set; }
    public DateTime DateAssessed { get; set; } = DateTime.Now;
    
    // reference to assignment the student was assessed for
    public int AssignmentId { get; set; }
    public int SubjectEnrollmentId { get; set; }
    [MaxLength(1000)]
    public string? Comment { get; set; }

    public Assessment(decimal gradeValue, string? comment = null) 
    {
        GradeValue = gradeValue;
        Comment = comment;
    }
}