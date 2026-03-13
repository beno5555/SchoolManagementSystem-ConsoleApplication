namespace SchoolManagementSystem.Data.Models.JoinedModels;

public class TeacherSubject
{
    public int TeacherId { get; set; }
    public int SubjectId { get; set; }
    
    // group in which the teacher teaches this specific subject
    public int GroupId { get; set; }   
}