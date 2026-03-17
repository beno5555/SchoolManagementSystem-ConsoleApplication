using System.ComponentModel.DataAnnotations;

namespace SchoolManagementSystem.Data.Models;

public class Subject : BaseModel
{

    // use Enums.SubjectName
    [MaxLength(100)] public string SubjectName { get; set; }  
    public string Description { get; set; } = string.Empty;
    
    public int LaboratoryId { get; set; }
    public Subject(string subjectName) : base()
    {
        SubjectName = subjectName;
    }
}