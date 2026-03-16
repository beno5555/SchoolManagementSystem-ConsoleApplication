using System.ComponentModel.DataAnnotations;

namespace SchoolManagementSystem.Data.Models;

public class Laboratory : BaseModel
{
    [MaxLength(50)]
    public string LaboratoryName { get; set; } 
    [MaxLength(1000)]
    public string Description { get; set; } 

    public Laboratory(int id, string laboratoryName, string description = "") : base(id)
    {
        LaboratoryName = laboratoryName;
        Description = description;
    }   
}