using SchoolManagementSystem.Data.Attributes;
using SchoolManagementSystem.Data.HelperClasses;
using SchoolManagementSystem.Data.Models.Base;

namespace SchoolManagementSystem.Data.Models;

[FileNamePrefix("roles")]
public class Role : NamedModel
{
    public Role(string roleName) : base(roleName) 
    {
    }

    public Role()
    {
        
    }
}