using SchoolManagementSystem.Data.Models.Base;
using SchoolManagementSystem.Data.HelperClasses;
namespace SchoolManagementSystem.Data.Models.JoinedModels;

[FileNamePrefix("classes")]
public class Class : BaseModel
{
    public int TeacherId { get; set; }
    public int SubjectId { get; set; }

    public Class()
    {
        
    }

    public Class(int teacherId, int subjectId)
    {
        TeacherId = teacherId;
        SubjectId = subjectId;
    }
}