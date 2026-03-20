using SchoolManagementSystem.Data.Models.Base;

namespace SchoolManagementSystem.Data.Models.JoinedModels;

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