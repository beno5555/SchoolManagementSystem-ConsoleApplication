namespace SchoolManagementSystem.Data.Models.JoinedModels;

// student data in a specific subjects subjects
public class SubjectEnrollment
{
    public int StudentId { get; set; }
    
    public int SubjectId { get; set; }
    public required Subject Subject { get; set; }

    public List<Assessment> Assessments { get; set; } = new(200);

    public decimal GetFinalGrade()
    {
        return Assessments.Any() ? Assessments.Average(grade => grade.GradeValue) : 0;
    }
}