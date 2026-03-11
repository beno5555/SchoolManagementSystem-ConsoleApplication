using SchoolManagementSystem.Data.Models.UserProfiles;

namespace SchoolManagementSystem.Data.Models;

public class Group
{
    private static int _idIncrement = 1;
    public int GroupId { get; set; } = _idIncrement++;
    public string GroupName { get; set; } = string.Empty;
    public required TeacherProfile SupervisorTeacher { get; set; }
    public Classroom? Classroom { get; set; }
}