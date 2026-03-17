using SchoolManagementSystem.Data.Models.Base;

namespace SchoolManagementSystem.Data.Models;

public class AssignmentType : BaseModel
{
    public string Name { get; set; } 

    public AssignmentType(string name) : base()
    {
        Name = name;
    }
}