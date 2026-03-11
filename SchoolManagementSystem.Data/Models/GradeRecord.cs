using SchoolManagementSystem.Data.Constants;

namespace SchoolManagementSystem.Data.Models;

public class GradeRecord
{
    public int GradeId { get; set; }
    public decimal GradeValue { get; set; }
    public DateTime DateAssigned { get; set; } = DateTime.Now;
    public Enums.AssessmentType AssessmentType { get; set; }
    
    public int StudentSubjectId { get; set; }
    
}