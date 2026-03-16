using System.ComponentModel.DataAnnotations;
using SchoolManagementSystem.Data.Constants;

namespace SchoolManagementSystem.Data.Models;

public class Assignment : BaseModel
{
    [MaxLength(70)]
    public string AssignmentName { get; set; } = string.Empty;

    public SchoolEnums.AssignmentStatus AssignmentStatus { get; set; } = SchoolEnums.AssignmentStatus.Pending;
    [MaxLength(500)]
    public string Description { get; set; } = string.Empty;

    public int GroupId { get; set; }

    public DateTime UploadedAt { get; set; } = DateTime.Now;
    public DateTime DueDate { get; set; }
    
}