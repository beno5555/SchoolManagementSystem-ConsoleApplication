using System.ComponentModel.DataAnnotations;
using SchoolManagementSystem.Data.Attributes;
using SchoolManagementSystem.Data.HelperClasses;
using SchoolManagementSystem.Data.Models.Base;

namespace SchoolManagementSystem.Data.Models;

[FileNamePrefix("subjects")]
public class Subject : BaseModel
{
    [MaxLength(100)] public string SubjectName { get; set; }  
    public string Description { get; set; } = string.Empty;
    
    [Reference<Room>]
    public int? LaboratoryId { get; set; }
    public Subject(string subjectName) 
    {
        SubjectName = subjectName;
    }
}