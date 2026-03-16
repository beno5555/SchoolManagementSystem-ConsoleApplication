using System.ComponentModel.DataAnnotations;

namespace SchoolManagementSystem.Data.Models;

public class Laboratory : BaseModel
{
    [MaxLength(50)]
    public string LaboratoryName { get; set; } = string.Empty;
    [MaxLength(1000)]
    public string Description { get; set; } = string.Empty;
    
}