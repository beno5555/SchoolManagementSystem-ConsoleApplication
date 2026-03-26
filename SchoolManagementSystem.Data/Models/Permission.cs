using SchoolManagementSystem.Data.Attributes;
using SchoolManagementSystem.Data.Models.Base;

namespace SchoolManagementSystem.Data.Models;

[FileNamePrefix("permissions")]
public class Permission : NamedModel
{
    public Permission(string permissionName) : base(permissionName) 
    {
    }

    public Permission()
    {
        
    }
}