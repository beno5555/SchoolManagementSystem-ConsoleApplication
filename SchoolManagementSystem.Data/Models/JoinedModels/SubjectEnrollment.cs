using SchoolManagementSystem.Data.HelperClasses;
using SchoolManagementSystem.Data.Models.Base;

namespace SchoolManagementSystem.Data.Models.JoinedModels;

// student data in a specific subjects subjects
[FileNamePrefix("subjectEnrollments")]
public class SubjectEnrollment : BaseModel
{
    public int StudentId { get; set; }

    public int ClassId { get; set; }    
    public decimal FinalGrade => Math.Round(AverageGrade);
    public decimal AverageGrade { get; set; }
    // public List<Assessment> Assessments { get; set; } = new(200);

    public SubjectEnrollment()
    {
        
    }

    public SubjectEnrollment(int studentId, int classId)
    {
        StudentId = studentId;
        ClassId = classId;
    }
}