using SchoolManagementSystem.Data.HelperClasses;

namespace SchoolManagementSystem.Data.Models.JoinedModels;
[FileNamePrefix("rolePermissions")]
public class RolePermission
{
    public int RoleId { get; set; }
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