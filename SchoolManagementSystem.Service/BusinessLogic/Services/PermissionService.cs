using SchoolManagementSystem.Service.BusinessLogic.Factories;

namespace SchoolManagementSystem.Service.BusinessLogic.Services;

public class PermissionService
{
    private UtilityFactory _utilities;

    public PermissionService(UtilityFactory utilities)
    {
        _utilities = utilities;
    }

    public async Task<bool> HasPermission(string roleName, string permission)
    {
        bool hasPermission = false;
        var roleId = await _utilities.Repos.RoleRepository.GetIdByName(roleName);

        if (roleId != -1)
        {
            var rolePermissionsResponse = await _utilities.Repos.RolePermissionRepository.GetPermissionsByRole(roleId);
            
            if (rolePermissionsResponse.Success)
            {
                var permissionIds = rolePermissionsResponse.Value.Select(rolePermission => rolePermission.PermissionId).ToList();
                var requestedPermissionResponse = await _utilities.Repos.PermissionRepository.GetByName(permission);

                if (requestedPermissionResponse.Success)
                {
                    hasPermission = permissionIds.Contains(requestedPermissionResponse.Value.Id);
                }
            }
        }

        return hasPermission;
    }
}