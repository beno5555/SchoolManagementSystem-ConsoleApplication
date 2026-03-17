namespace SchoolManagementSystem.Data.Models.UserProfiles;

class StudentProfile : User
{
    #region Properties

    public int GroupId { get; set; }
    
    #endregion
    public StudentProfile(
        string firstName, string lastName, DateTime dateOfBirth, string privateId, string email, string passwordHash,
        int groupId) : base(firstName, lastName, dateOfBirth, privateId, email, passwordHash, 4) // 4 for student
    {
        GroupId = GroupId;
    }

    public int FinalStudentGrade { get; set; }
    
    // implement this in one of the services instead of a model 
    // public decimal GetAverageGrade()
    // {
        // return SubjectEnrollments.Any() ? Math.Round(subjectEnrollments.Average(enrollment => enrollment.FinalGrade), 2) : 0;
    // }

}