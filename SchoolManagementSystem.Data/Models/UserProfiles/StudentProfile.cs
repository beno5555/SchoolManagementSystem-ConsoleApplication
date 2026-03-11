using SchoolManagementSystem.Data.Models.JoinedModels;

namespace SchoolManagementSystem.Data.Models.UserProfiles;

public class StudentProfile
{
    public List<Subject> Subjects { get; set; } = [];
    public List<Enrollment> Enrollments { get; set; } = [];


    public decimal GetAverageGrade()
    {
        return Enrollments.Any() ? Enrollments.Average(enrollment => enrollment.FinalGrade) : 0;
    }

}