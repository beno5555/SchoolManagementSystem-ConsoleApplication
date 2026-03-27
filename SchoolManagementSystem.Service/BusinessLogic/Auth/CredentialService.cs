using ProjectHelperLibrary.Response;
using ProjectHelperLibrary.Utilities;
using SchoolManagementSystem.Data.Models;
using SchoolManagementSystem.Data.Repositories;
using SchoolManagementSystem.Service.DTOs.User.Auth;
using SchoolManagementSystem.Service.DTOs.User.Display;
using SchoolManagementSystem.Service.Mapping;

namespace SchoolManagementSystem.Service.BusinessLogic.Auth;

public class CredentialService
{
    private readonly UserRepository _userRepository = new();
    private readonly Mapper _mapper = new();
    private readonly PasswordHasher _passwordHasher = new();
    #region Methods
    
    public async Task<DataResponse<User>> PrepareForRegistration(BaseRegisterDTO registerDTO)
    {
        DataResponse<User> response = new();
        var credentialValidationResponse = await AreCredentialsUnique(registerDTO.Email, registerDTO.PrivateId);

        if (credentialValidationResponse.Success)
        {
            var userToRegister = _mapper.RegisterDTOToUser(registerDTO);
            response.SetData(userToRegister);
        }
        else
        {
            response.SetStatus(false, credentialValidationResponse.Message);
        }
        return response;
    }

    private async Task<BaseResponse> AreCredentialsUnique(string email, string privateId)
    {
        BaseResponse response = new();
        bool existsByEmail = await _userRepository.ExistsByEmail(email);
        if (existsByEmail)
        {
            response.SetStatus(false, "Email is already taken");
        }
        else
        {
            bool existsByPrivateId = await _userRepository.ExistsByPrivateId(privateId);
            if (existsByPrivateId)
            {
                response.SetStatus(false, "User with private id is already in the database");
            }
        }

        return response;
    }

    public async Task<DataResponse<UserDisplayDTO>> AuthenticateUser(User userToAuthenticate, string password)
    {
        DataResponse<UserDisplayDTO> response = new();

        var validPasswordResponse = _passwordHasher.VerifyPassword(
            password, 
            userToAuthenticate.PasswordHash, 
            userToAuthenticate.PasswordSalt);
                
        if (validPasswordResponse.Success)
        {
            var userDisplayDTO = await _mapper.UserToDisplayDTO(userToAuthenticate);
            if (userDisplayDTO is not null)
            {
                response.SetData(userDisplayDTO);
            }
            else
            {
                response.SetStatus(false, "Could not identify role (temporary message)");
            }
        }
        else
        {
            response.SetStatus(false, $"{validPasswordResponse.Message} or email");
        }

        return response;
    }
    
    public async Task<DataResponse<User>> GetUserByUniqueIdentifier(LoginDTO loginDTO)
    {
        DataResponse<User> response = new();
        if (!string.IsNullOrWhiteSpace(loginDTO.Email))
        {
            response = await _userRepository.GetByEmail(loginDTO.Email);
        }
        else if (!string.IsNullOrWhiteSpace(loginDTO.PrivateId))
        {
            response = await _userRepository.GetByPrivateId(loginDTO.PrivateId);
        }
        else
        {
            response.SetStatus(false, 
                $"Missing unique identifier ({nameof(loginDTO.Email)} or {nameof(loginDTO.PrivateId)})");
        }

        return response;
    }
    
    #endregion
}

