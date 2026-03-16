using SchoolManagementSystem.Data.Constants;

namespace SchoolManagementSystem.Data.Models;

public class Role : BaseModel
{
    public string RoleName { get; set; } 

    public Role(int id, string roleName) : base(id)
    {
        RoleName = roleName;
    }
}