using System.ComponentModel.DataAnnotations;
using SchoolManagementSystem.Data.Constants;
using SchoolManagementSystem.Data.Models.UserProfiles;

namespace SchoolManagementSystem.Data.Models;

// a single recorded assessment
public class Assessment : BaseModel
{
    [Range(0, 10)]
    public decimal GradeValue { get; set; }
    public DateTime DateAssessed { get; set; } = DateTime.Now;
    
    // reference to assignment the student was assessed for
    public int AssignmentId { get; set; } 
    public SchoolEnums.AssessmentType AssessmentType { get; set; }
    [MaxLength(1000)]
    public string? Comment { get; set; }
}