using SchoolManagementSystem.Data.Attributes;
using SchoolManagementSystem.Data.HelperClasses;
using SchoolManagementSystem.Data.Models.Base;

namespace SchoolManagementSystem.Data.Models.JoinedModels;

// student data in a specific subjects subjects
[FileNamePrefix("subjectEnrollments")]
public class SubjectEnrollment : BaseModel
{
    [Reference<User>]
    public int StudentId { get; set; }
    
    [Reference<SchoolClass>]
    public int SchoolClassId { get; set; }    
    public decimal FinalGrade => Math.Round(AverageGrade);
    public decimal AverageGrade { get; set; }
    // public List<Assessment> Assessments { get; set; } = new(200);

    public SubjectEnrollment()
    {
        
    }

    public SubjectEnrollment(int studentId, int schoolClassId)
    {
        StudentId = studentId;
        SchoolClassId = schoolClassId;
    }
}