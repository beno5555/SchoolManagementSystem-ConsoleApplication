using SchoolManagementSystem.Data.Attributes;
using SchoolManagementSystem.Data.Models.Named;

namespace SchoolManagementSystem.Data.Models.JoinedModels;

[FileNamePrefix("rolePermissions")]
public class RolePermission
{
    [Reference<Role>]
    public int RoleId { get; set; }
    
    [Reference<Permission>]
    public int PermissionId { get; set; }

    public RolePermission(int roleId, int permissionId)
    {
        RoleId = roleId;
        PermissionId = permissionId;
    }
    public RolePermission()
    {
        
    }
}