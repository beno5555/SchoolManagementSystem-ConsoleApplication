using ProjectHelperLibrary.Response;
using SchoolManagementSystem.Data.Models.JoinedModels;
using SchoolManagementSystem.Data.Repositories.Base;

namespace SchoolManagementSystem.Data.Repositories.Joined;

public class RolePermissionRepository : FileRepository<RolePermission>
{
    public RolePermissionRepository() : base(SchoolContext.RolePermissions)
    {
        
    }

    public async Task<DataResponse<List<RolePermission>>> GetPermissionsByRole(int roleId)
    {
        return await GetWhere(
            rolePermission => rolePermission.RoleId == roleId,
            "Could not find any Permissions associated with the role");
    }
}