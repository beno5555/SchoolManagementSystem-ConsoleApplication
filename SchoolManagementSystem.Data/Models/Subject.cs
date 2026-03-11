namespace SchoolManagementSystem.Data.Models;

public class Subject
{
    private static int _idIncrement = 1;
    public int SubjectId { get; set; } = _idIncrement++;
    public string SubjectName { get; set; } = string.Empty;

    public List<User> Students { get; set; } = [];
    public List<User> Teachers { get; set; } = [];
}