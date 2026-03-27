using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using SchoolManagementSystem.Data.Attributes;
using SchoolManagementSystem.Data.Config;
using SchoolManagementSystem.Data.Models.Base;
using SchoolManagementSystem.Data.Models.JoinedModels;

namespace SchoolManagementSystem.Data.Models;

[FileNamePrefix("assignments")]
public class Assignment : BaseModel

{
    [MaxLength(70)]
    public string AssignmentName { get; set; } 

    [Reference<AssignmentType>]
    public int AssignmentTypeId { get; set; }
    
    public SchoolEnums.AssignmentStatus AssignmentStatus { get; set; } = SchoolEnums.AssignmentStatus.Pending;
    [MaxLength(500)]
    public string Description { get; set; } = string.Empty;
    
    [Reference<GroupClass>]
    public int GroupClassId { get; set; }

    public DateTime UploadedAt { get; set; } = DateTime.Now;
    public DateTime DueDate { get; set; }
    
    [JsonIgnore]
    public bool IsOverdue => AssignmentStatus == SchoolEnums.AssignmentStatus.Pending && DueDate < DateTime.Now;

    public Assignment(string name, int assignmentTypeId,  DateTime dueDate, string description = "")
    {
        AssignmentName = name;
        AssignmentTypeId = assignmentTypeId;
        Description = description;
        DueDate = dueDate;
    }
}