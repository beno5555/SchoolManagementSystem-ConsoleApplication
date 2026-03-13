using SchoolManagementSystem.Data.Constants;
using SchoolManagementSystem.Data.Models.JoinedModels;

namespace SchoolManagementSystem.Data.Models.UserProfiles;

public class StudentProfile : User
{
    #region Properties

    public List<Subject> Subjects { get; set; } = new(20);
    public int GroupId { get; set; }
    
    public List<SubjectEnrollment> Enrollments { get; set; } = [];
    
    #endregion
    public StudentProfile(/* shared parameters: */ 
        string firstName, string lastName, DateTime dateOfBirth, string privateId, string email, string passwordHash,
        /* student parameters: */
        int groupId) : base(firstName, lastName, dateOfBirth, privateId, email, passwordHash, Enums.RoleName.Teacher)
    {
        GroupId = GroupId;
    }

    public decimal FinalStudentGrade => Math.Round(GetAverageGrade());
    public decimal GetAverageGrade()
    {
        return Enrollments.Any() ? Math.Round(Enrollments.Average(enrollment => enrollment.FinalGrade), 2) : 0;
    }

}