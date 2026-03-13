using System.ComponentModel.DataAnnotations;
using SchoolManagementSystem.Data.Constants;

namespace SchoolManagementSystem.Data.Models;

public class Assignment
{
    private static int _idIncrement = 1;
    public int AssignmentId { get; set; } = _idIncrement++;
    [MaxLength(70)]
    public string AssingmentName { get; set; } = string.Empty;

    public Enums.AssignmentStatus AssignmentStatus { get; set; } = Enums.AssignmentStatus.Pending;
    [MaxLength(500)]
    public string Description { get; set; } = string.Empty;

    public int GroupId { get; set; }

    public DateTime UploadedAt { get; set; } = DateTime.Now;
    public DateTime DueDate { get; set; }
    
}