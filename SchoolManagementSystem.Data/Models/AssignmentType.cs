using SchoolManagementSystem.Data.Attributes;
using SchoolManagementSystem.Data.HelperClasses;
using SchoolManagementSystem.Data.Models.Base;

namespace SchoolManagementSystem.Data.Models;

[FileNamePrefix("assignmentTypes")]
public class AssignmentType : NamedModel
{

    public AssignmentType(string name) : base(name) 
    {
        
    }

    public AssignmentType()
    {
        
    }
}