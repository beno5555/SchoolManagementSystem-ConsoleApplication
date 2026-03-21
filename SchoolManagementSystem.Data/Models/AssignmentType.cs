using SchoolManagementSystem.Data.HelperClasses;
using SchoolManagementSystem.Data.Models.Base;

namespace SchoolManagementSystem.Data.Models;

[FileNamePrefix("assignmentTypes")]
public class AssignmentType : BaseModel
{
    public string Name { get; set; } 

    public AssignmentType(string name) 
    {
        Name = name;
    }
}