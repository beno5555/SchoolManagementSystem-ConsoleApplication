using System.ComponentModel.DataAnnotations;
using SchoolManagementSystem.Data.Models.Base;

namespace SchoolManagementSystem.Data.Models;

public class Subject : BaseModel
{
    [MaxLength(100)] public string SubjectName { get; set; }  
    public string Description { get; set; } = string.Empty;
    
    public int? LaboratoryId { get; set; }
    public Subject(string subjectName) 
    {
        SubjectName = subjectName;
    }
}