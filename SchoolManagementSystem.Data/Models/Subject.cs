using System.ComponentModel.DataAnnotations;

namespace SchoolManagementSystem.Data.Models;

public class Subject
{
    private static int _idIncrement = 1;
    public int SubjectId { get; set; } = _idIncrement++;

    // use Enums.SubjectName
    [MaxLength(100)] public string SubjectName { get; set; } = string.Empty; 
    public string Description { get; set; } = string.Empty;
    
    public int LaboratoryId { get; set; }
}