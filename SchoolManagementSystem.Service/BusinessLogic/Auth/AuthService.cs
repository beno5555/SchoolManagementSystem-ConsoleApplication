using ProjectHelperLibrary.Response;
using SchoolManagementSystem.Data.Config;
using SchoolManagementSystem.Data.Models;
using SchoolManagementSystem.Data.Repositories;
using SchoolManagementSystem.Service.DTOs.User.Auth;
using SchoolManagementSystem.Service.DTOs.User.Display;
using SchoolManagementSystem.Service.Mapping;

namespace SchoolManagementSystem.Service.BusinessLogic.Auth;

public class AuthService
{
    private readonly UserRepository _userRepository = new();
    private readonly GroupRepository _groupRepository = new();
    private readonly RoleRepository _roleRepository = new();
    private readonly CredentialService _credentialService = new();
    
    #region Registration
    
    /// <summary>
    /// meant to be used only by the admin to register a new user
    /// </summary>
    public async Task<BaseResponse> RegisterUser(AdminRegisterDTO adminRegisterDTO)
    {
        var response = await Register(adminRegisterDTO, async userToRegister =>
        {
            bool isValidRole = await _roleRepository.ExistsAsync(adminRegisterDTO.RoleId);
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
    /// self register (only as a user)
    /// </summary>
    public async Task<BaseResponse> RegisterStudent(StudentRegisterDTO studentRegisterDTO)
    {
        var response = await Register(studentRegisterDTO, async studentToRegister =>
        {
            int studentRoleId = await _roleRepository.GetIdByName(nameof(SchoolEnums.RoleName.Student));
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
    public async Task<BaseResponse> Register<T>(T registerDTO, Func<User, Task<bool>> assignSpecifics)
    where T : BaseRegisterDTO
    {
        BaseResponse response = new();
        var validationResponse = await _credentialService.ValidateRegisterCredentials(registerDTO);

        if (validationResponse.Success)
        {
            var userToRegister = validationResponse.Value;
            var specificsSuccess = await assignSpecifics(userToRegister);
            
            if (specificsSuccess)
            {
                await _userRepository.AddAsync(userToRegister);
            }
            else
            {
                response.SetStatus(false, "Invalid role");
            }
        }
        else
        {
            response.SetStatus(false, validationResponse.Message);
        }

        return response;
    }
    
    #endregion
    
    #region Login
    
    public async Task<DataResponse<UserDisplayDTO>> Login(LoginDTO loginDTO)
    {
        DataResponse<UserDisplayDTO> response = new();
        var userResponse = await _credentialService.GetUserByUniqueIdentifier(loginDTO);
        
        if (userResponse.Success)
        {
            response = await _credentialService.AuthenticateUser(userResponse.Value, loginDTO.Password);
        }
        else
        {
            response.SetStatus(false, userResponse.Message);
        }

        return response;
    }
    
    #endregion
}