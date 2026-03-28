using System.ComponentModel.DataAnnotations;
using SchoolManagementSystem.Data.Attributes;
using SchoolManagementSystem.Data.Models.Base;
using SchoolManagementSystem.Data.Models.JoinedModels;

namespace SchoolManagementSystem.Data.Models.Academic;

[FileNamePrefix("assessments")]
public class Assessment : BaseModel
{
    public decimal GradeValue { get; set; }
    public string? VerbalAssessment { get; set; }
    public DateTime DateAssessed { get; set; } = DateTime.Now;
    
    [Reference<Submission>]
    public int SubmissionId { get; set; }

    [Reference<Assignment>]
    public int AssignmentId { get; set; }
}