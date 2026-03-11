namespace SchoolManagementSystem.Data.Models.JoinedModels;

public class Enrollment
{
    public int StudentId { get; set; }
    public int SubjectId { get; set; }
    
    public List<GradeRecord> Grades { get; set; } = [];
    public decimal FinalGrade => Grades.Any() ? Grades.Average(g => g.GradeValue) : 0;
}