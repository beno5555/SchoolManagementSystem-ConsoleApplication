using SchoolManagementSystem.Data.HelperClasses;
using SchoolManagementSystem.Data.Models.Base;

namespace SchoolManagementSystem.Data.Models;

[FileNamePrefix("permissions")]
public class Permission : BaseModel
{
    public string PermissionName { get; set; } 
    public Permission(string permissionName) : base()
    {
        PermissionName = permissionName;
    }
}