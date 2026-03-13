using SchoolManagementSystem.Data.Models.UserProfiles;

namespace SchoolManagementSystem.Data.Models;

public class Group
{
    private static int _idIncrement = 1;
    public int GroupId { get; set; } = _idIncrement++;
    public string GroupName { get; set; } = string.Empty;
    
    public int TeacherId { get; set; }
    public int ClassroomId { get; set; }
}