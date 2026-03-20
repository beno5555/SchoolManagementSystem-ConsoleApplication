using SchoolManagementSystem.Data.HelperClasses;
using SchoolManagementSystem.Data.Models.Base;

namespace SchoolManagementSystem.Data.Models;

[FileNamePrefix("roles")]
public class Role : BaseModel
{
    public string RoleName { get; set; } 

    public Role(string roleName) : base()
    {
        RoleName = roleName;
    }
}