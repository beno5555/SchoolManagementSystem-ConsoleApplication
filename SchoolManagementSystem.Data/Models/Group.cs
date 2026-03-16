namespace SchoolManagementSystem.Data.Models;

public class Group : BaseModel
{
    public string GroupName { get; set; } 
    
    public int TeacherId { get; set; }
    public int ClassroomId { get; set; }

    public Group(int id, string groupName) : base(id)
    {
        GroupName = groupName;
    }
}