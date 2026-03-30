using SchoolManagementSystem.Data.Attributes;
using SchoolManagementSystem.Data.Models.Base;
using SchoolManagementSystem.Data.Models.Named;

namespace SchoolManagementSystem.Data.Models.JoinedModels;

[FileNamePrefix("schoolClasses")]
public class SchoolClass : NamedModel
{
    [Reference<User>]
    public int TeacherId { get; set; }
    
    [Reference<Subject>]
    public int SubjectId { get; set; }

    public SchoolClass()
    {
        
    }

    public SchoolClass(int teacherId, int subjectId)
    {
        TeacherId = teacherId;
        SubjectId = subjectId;
    }
}