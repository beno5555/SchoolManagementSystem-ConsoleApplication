using SchoolManagementSystem.Data.Attributes;
using SchoolManagementSystem.Data.Models.Base;

namespace SchoolManagementSystem.Data.Models;

[FileNamePrefix("subjects")]
public class Subject : NamedModel
{
    public string Description { get; set; } = string.Empty;
    
    [Reference<Room>]
    public int? LaboratoryId { get; set; }
}