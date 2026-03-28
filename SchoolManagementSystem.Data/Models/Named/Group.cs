using SchoolManagementSystem.Data.Attributes;
using SchoolManagementSystem.Data.Models.Base;

namespace SchoolManagementSystem.Data.Models.Named;

[FileNamePrefix("groups")]
public class Group : NamedModel
{
    
    // damrigebeli
    [Reference<User>]
    public int TeacherId { get; set; }
    
    [Reference<Room>]
    public int ClassroomId { get; set; }
}