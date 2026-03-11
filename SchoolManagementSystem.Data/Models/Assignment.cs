using System.ComponentModel.DataAnnotations;

namespace SchoolManagementSystem.Data.Models;

public class Assignment
{
    private static int _idIncrement = 1;
    public int AssignmentId { get; set; } = _idIncrement++;
    [MaxLength(70)]
    public string AssingmentName { get; set; } = string.Empty;
    [MaxLength(500)]
    public string Description { get; set; } = string.Empty;

    public int GroupId { get; set; }
    public Group? Group { get; set; }

    public DateTime UploadedAt { get; set; } = DateTime.Now;
    public DateTime DueDate { get; set; }

    
    public List<Assessment> Assessments { get; set; } = new(50);
}