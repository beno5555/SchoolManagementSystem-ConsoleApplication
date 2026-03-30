using ProjectHelperLibrary.Response;
using SchoolManagementSystem.Data.Models;
using SchoolManagementSystem.Data.Models.Named;
using SchoolManagementSystem.Service.BusinessLogic.Factories;
using SchoolManagementSystem.Service.DTOs.User.Display;

namespace SchoolManagementSystem.Service.BusinessLogic.Services;

public class UserService
{
    private readonly UtilityFactory _utilities;

    public UserService(UtilityFactory utilities)
    {
        _utilities = utilities;
    }

    public async Task<DataResponse<List<Role>>> GetRoles() => await _utilities.Repos.RoleRepository.GetAll();

    public async Task<DataResponse<List<User>>> GetByRole(string roleName)
    {
        var response = new DataResponse<List<User>>();
        var roleId = await _utilities.Repos.RoleRepository.GetIdByName(roleName);

        if (roleId != -1)
        {
            response = await _utilities.Repos.UserRepository.GetByRoleId(roleId);
        }
        else
        {
            response.SetStatus(false, "Could not find role");
        }

        return response;
    }
}