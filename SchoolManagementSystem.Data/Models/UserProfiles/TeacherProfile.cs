using SchoolManagementSystem.Data.Constants;

namespace SchoolManagementSystem.Data.Models.UserProfiles;

public class TeacherProfile : User
{
    public List<Subject> Subjects { get; set; } = new(5);
    public Group? Group { get; set; } 
    public TeacherProfile(Person person, string email, string passwordHash) : base(person, email, passwordHash, Enums.RoleName.Teacher)
    {
        
    }
}