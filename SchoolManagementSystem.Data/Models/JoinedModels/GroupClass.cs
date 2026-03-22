using SchoolManagementSystem.Data.HelperClasses;
using SchoolManagementSystem.Data.Models.Base;

namespace SchoolManagementSystem.Data.Models.JoinedModels;

[FileNamePrefix("groupClasses")]
public class GroupClass : BaseModel
{
    public int GroupId { get; set; }
    public int ClassId { get; set; }
}