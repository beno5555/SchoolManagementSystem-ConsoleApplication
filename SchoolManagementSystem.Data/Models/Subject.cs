using SchoolManagementSystem.Data.Constants;
using SchoolManagementSystem.Data.Models.UserProfiles;

namespace SchoolManagementSystem.Data.Models;

public class Subject
{
    private static int _idIncrement = 1;
    public int SubjectId { get; set; } = _idIncrement++;
    public Enums.SubjectName SubjectName { get; set; }

    public List<StudentProfile> Students { get; set; } = [];
    public List<TeacherProfile> Teachers { get; set; } = [];
}