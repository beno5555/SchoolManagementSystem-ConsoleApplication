namespace SchoolManagementSystem.Data.Models;

public class Classroom
{
    private static int _idIncrement = 1;
    public int ClassroomId { get; set; } = _idIncrement++;
    public string ClassroomName { get; set; } = string.Empty;
    public List<Group> Groups { get; set; } = new(4);
}