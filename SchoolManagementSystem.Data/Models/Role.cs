using SchoolManagementSystem.Data.Models.Base;

namespace SchoolManagementSystem.Data.Models;

public class Role : BaseModel
{
    public string RoleName { get; set; } 

    public Role(string roleName) : base()
    {
        RoleName = roleName;
    }
}