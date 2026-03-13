using SchoolManagementSystem.Data.Constants;

namespace SchoolManagementSystem.Data.Models.UserProfiles;

public class TeacherProfile : User
{
    public List<Subject> Subjects { get; set; } = new(5);
    public Group? Group { get; set; } 
    public TeacherProfile(string firstName, string lastName, DateTime dateOfBirth, string privateId, string email, string passwordHash) : base(firstName, lastName, dateOfBirth, privateId, email, passwordHash, Enums.RoleName.Teacher)
    {
        
    }
}