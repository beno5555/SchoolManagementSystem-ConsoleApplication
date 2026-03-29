using ProjectHelperLibrary.Response;
using SchoolManagementSystem.Data.Config;
using SchoolManagementSystem.Data.Models;
using SchoolManagementSystem.Service.BusinessLogic.Factories;
using SchoolManagementSystem.Service.DTOs.User.Auth;

namespace SchoolManagementSystem.Service.BusinessLogic.Services;

public class AuthService
{
    private readonly UtilityFactory _utilities;

    public AuthService(UtilityFactory utilities)
    {
        _utilities = utilities;
    }
    
    #region Registration
    
    /// <summary>
    /// meant to be used only by the admin to register a new user
    /// </summary>
    public async Task<BaseResponse> RegisterUser(AdminRegisterDTO adminRegisterDTO)
    {
        var response = await Register(adminRegisterDTO, async userToRegister =>
        {
            bool isValidRole = await _utilities.Repos.RoleRepository.ExistsAsync(adminRegisterDTO.RoleId);
            if (isValidRole)
            {
                userToRegister.RoleId = adminRegisterDTO.RoleId;
                userToRegister.GroupId = adminRegisterDTO.GroupId;
                userToRegister.OfficeRoomId = adminRegisterDTO.OfficeRoomId;
            }

            return isValidRole;
        });
        return response;
    }

    /// <summary>
    /// self register (only as a student)
    /// </summary>
    public async Task<BaseResponse> RegisterStudent(StudentRegisterDTO studentRegisterDTO)
    {
        var response = await Register(studentRegisterDTO, async studentToRegister =>
        {
            int studentRoleId = await _utilities.Repos.RoleRepository.GetIdByName(nameof(SchoolEnums.RoleName.Student));
            bool isValidRole = studentRoleId != -1;
            if (isValidRole)
            {
                studentToRegister.RoleId = studentRoleId;
                studentToRegister.FinalGrade = 0;
            }
            return isValidRole;
        });
        return response;
    }

    /// <summary>
    /// generic register method
    /// </summary>
    /// <returns>success and message</returns>
    private async Task<BaseResponse> Register<T>(T registerDTO, Func<User, Task<bool>> assignSpecifics)
    where T : BaseRegisterDTO
    {
        BaseResponse response = new();
        var prepareResponse = await _utilities.IdentityService.PrepareForRegistration(registerDTO);
        if (prepareResponse.Success)
        {
            User userToRegister = prepareResponse.Value;
            bool specificsSuccess = await assignSpecifics(userToRegister);
            
            if (specificsSuccess)
            {
                await _utilities.Repos.UserRepository.AddAsync(userToRegister);
            }
            else
            {
                response.SetStatus(false, "Invalid role");
            }
        }
        else
        {
            response.SetStatus(false, prepareResponse.Message);
        }


        return response;
    }
    
    #endregion
    
    #region SignIn
    
    public async Task<DataResponse<SessionUser?>> SignIn(LoginDTO loginDTO)
    {
        DataResponse<SessionUser?> response = new();
        var userResponse = await _utilities.IdentityService.GetUserByUniqueIdentifier(loginDTO);
        
        if (userResponse.Success)
        {
            response = await _utilities.IdentityService.Authenticate(userResponse.Value, loginDTO.Password);
        }
        else
        {
            response.SetStatus(false, userResponse.Message);
        }
        
        return response;
    }
    
    #endregion
}