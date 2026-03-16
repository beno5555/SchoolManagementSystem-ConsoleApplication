using System.ComponentModel.DataAnnotations;
using SchoolManagementSystem.Data.Constants;

namespace SchoolManagementSystem.Data.Models;

public class Assignment : BaseModel
{
    [MaxLength(70)]
    public string AssignmentName { get; set; } 

    public SchoolEnums.AssignmentStatus AssignmentStatus { get; set; } = SchoolEnums.AssignmentStatus.Pending;
    public int AssignmentTypeId { get; set; }
    [MaxLength(500)]
    public string Description { get; set; } = string.Empty;
    
    public int GroupId { get; set; }

    public DateTime UploadedAt { get; set; } = DateTime.Now;
    public DateTime DueDate { get; set; }

    public Assignment(int id, string name, int assignmentTypeId, string description, DateTime dueDate) : base(id)
    {
        AssignmentName = name;
        AssignmentTypeId = assignmentTypeId;
        Description = description;
        DueDate = dueDate;
    }

    public Assignment(int id, string name, int assignmentTypeId, DateTime dueDate) : base(id)
    {
        AssignmentName = name;
        AssignmentTypeId = assignmentTypeId;
        DueDate = dueDate;
    }
}