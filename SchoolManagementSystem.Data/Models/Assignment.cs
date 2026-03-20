using System.ComponentModel.DataAnnotations;
using SchoolManagementSystem.Data.Config;
using SchoolManagementSystem.Data.HelperClasses;
using SchoolManagementSystem.Data.Models.Base;

namespace SchoolManagementSystem.Data.Models;

[FileNamePrefix("assignments")]
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

    public Assignment(string name, int assignmentTypeId,  DateTime dueDate, string description = "") : base()
    {
        AssignmentName = name;
        AssignmentTypeId = assignmentTypeId;
        Description = description;
        DueDate = dueDate;
    }
}