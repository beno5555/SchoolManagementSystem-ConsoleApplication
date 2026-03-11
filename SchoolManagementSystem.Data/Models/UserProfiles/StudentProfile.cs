using SchoolManagementSystem.Data.Constants;
using SchoolManagementSystem.Data.Models.JoinedModels;

namespace SchoolManagementSystem.Data.Models.UserProfiles;

public class StudentProfile : User
{
    #region Properties

    public List<Subject> Subjects { get; set; } = new(20);
    public Group Group { get; set; }
    public Classroom Classroom { get; set; }
    
    public List<SubjectEnrollment> Enrollments { get; set; } = [];
    
    #endregion
    public StudentProfile(Person person, string email, string passwordHash) : base(person, email, passwordHash, Enums.RoleName.Student)
    {
    }
    
    public decimal GetAverageGrade()
    {
        return Enrollments.Any() ? Enrollments.Average(enrollment => enrollment.GetFinalGrade()) : 0;
    }

}