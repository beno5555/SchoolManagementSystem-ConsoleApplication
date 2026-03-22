using SchoolManagementSystem.Data.Attributes;
using SchoolManagementSystem.Data.HelperClasses;
using SchoolManagementSystem.Data.Models.Base;

namespace SchoolManagementSystem.Data.Models;

[FileNamePrefix("groups")]
public class Group : BaseModel
{
    public string GroupName { get; set; } 
    
    // damrigebeli
    [Reference<User>]
    public int TeacherId { get; set; }
    
    [Reference<Room>]
    public int ClassroomId { get; set; }

    public Group(string groupName) 
    {
        GroupName = groupName;
    }
}