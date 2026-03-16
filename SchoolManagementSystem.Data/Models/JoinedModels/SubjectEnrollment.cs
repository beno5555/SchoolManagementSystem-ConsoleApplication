namespace SchoolManagementSystem.Data.Models.JoinedModels;

// student data in a specific subjects subjects
public class SubjectEnrollment
{
    public int StudentId { get; set; }
    public int SubjectId { get; set; }
    public int TeacherId { get; set; } // in that subject
    
    
    public List<Assessment> Assessments { get; set; } = new(200);
    public decimal FinalGrade => Math.Round(GetAverageGrade());

    public decimal GetAverageGrade()
    {
        return Assessments.Any() ? Math.Round(Assessments.Average(grade => grade.GradeValue), 2) : 0;
    }
}