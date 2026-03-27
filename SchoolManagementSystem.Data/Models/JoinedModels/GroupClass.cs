using SchoolManagementSystem.Data.Attributes;
using SchoolManagementSystem.Data.Models.Base;

namespace SchoolManagementSystem.Data.Models.JoinedModels;

[FileNamePrefix("groupClasses")]
public class GroupClass : BaseModel
{
    [Reference<Group>]
    public int GroupId { get; set; }
    
    [Reference<SchoolClass>]
    public int SchoolClassId { get; set; }
}