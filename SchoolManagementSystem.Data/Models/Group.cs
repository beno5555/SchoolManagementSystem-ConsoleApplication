using SchoolManagementSystem.Data.HelperClasses;
using SchoolManagementSystem.Data.Models.Base;

namespace SchoolManagementSystem.Data.Models;

[FileNamePrefix("groups")]
public class Group : BaseModel
{
    public string GroupName { get; set; } 
    
    // damrigebeli
    public int TeacherId { get; set; }
    public int ClassroomId { get; set; }

    public Group(string groupName) : base()
    {
        GroupName = groupName;
    }
}