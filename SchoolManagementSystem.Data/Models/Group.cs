using SchoolManagementSystem.Data.Models.UserProfiles;

namespace SchoolManagementSystem.Data.Models;

public class Group : BaseModel
{
    public string GroupName { get; set; } = string.Empty;
    
    public int TeacherId { get; set; }
    public int ClassroomId { get; set; }
}