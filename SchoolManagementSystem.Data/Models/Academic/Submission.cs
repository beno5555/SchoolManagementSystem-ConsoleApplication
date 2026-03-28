using SchoolManagementSystem.Data.Attributes;
using SchoolManagementSystem.Data.Config;
using SchoolManagementSystem.Data.Models.Base;
using SchoolManagementSystem.Data.Models.JoinedModels;

namespace SchoolManagementSystem.Data.Models.Academic;

[FileNamePrefix("submissions")]
public class Submission : BaseModel
{
    public string SubmissionContent { get; set; } = string.Empty;
    public string StudentComment { get; set; } = string.Empty;
    public SchoolEnums.SubmissionStatus SubmissionStatus { get; set; }
    
    [Reference<Assignment>] 
    public int AssignmentId { get; set; }

    [Reference<SubjectEnrollment>]
    public int SubjectEnrollmentId{ get; set; }

    public void MarkSubmitted(bool isOverDue)
    {
        SubmissionStatus = isOverDue 
                ? SchoolEnums.SubmissionStatus.SubmittedLate 
                : SchoolEnums.SubmissionStatus.Submitted;
    }

    public void MarkReviewed()
    {
        SubmissionStatus = SubmissionStatus switch
        {
            SchoolEnums.SubmissionStatus.Submitted => SchoolEnums.SubmissionStatus.Reviewed,
            SchoolEnums.SubmissionStatus.SubmittedLate => SchoolEnums.SubmissionStatus.ReviewedLate,
            _ => SubmissionStatus
        };
    }
}