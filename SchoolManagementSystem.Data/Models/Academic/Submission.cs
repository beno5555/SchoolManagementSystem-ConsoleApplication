using System.Runtime.CompilerServices;
using SchoolManagementSystem.Data.Attributes;
using SchoolManagementSystem.Data.Models.Base;
using SchoolManagementSystem.Data.Models.JoinedModels;

namespace SchoolManagementSystem.Data.Models.Academic;

[FileNamePrefix("submissions")]
public class Submission : BaseModel
{
    public string SubmissionContent { get; set; } = string.Empty;
    public string StudentComment { get; set; } = string.Empty;
    
    [Reference<Assignment>] 
    public int AssignmentId { get; set; }

    [Reference<SubjectEnrollment>]
    public int SubjectEnrollmentId{ get; set; }

}