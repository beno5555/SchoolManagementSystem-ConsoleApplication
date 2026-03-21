using SchoolManagementSystem.Data.HelperClasses;

namespace SchoolManagementSystem.Data.Models.JoinedModels;

[FileNamePrefix("groupClasses")]
public class GroupClass
{
    public int GroupId { get; set; }
    public int ClassId { get; set; }
}