using System.ComponentModel.DataAnnotations;

namespace SchoolManagementSystem.Data.Models;

public class Subject : BaseModel
{

    // use Enums.SubjectName
    [MaxLength(100)] public string SubjectName { get; set; } = string.Empty; 
    public string Description { get; set; } = string.Empty;
    
    public int LaboratoryId { get; set; }
    public Subject(int id, string subjectName)
    {
        Id = id;
        SubjectName = subjectName;
    }
}