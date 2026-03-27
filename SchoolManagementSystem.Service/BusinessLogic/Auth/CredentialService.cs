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
    
    public async Task<DataResponse<User>> ValidateRegisterCredentials(BaseRegisterDTO registerDTO)
    {
        DataResponse<User> response = new();
        bool existsByEmail = await _userRepository.ExistsByEmail(registerDTO.Email);
        if (!existsByEmail)
        {
            bool existsByPrivateId = await _userRepository.ExistsByPrivateId(registerDTO.PrivateId);

            if (!existsByPrivateId)
            {
                var userToRegister = _mapper.RegisterDTOToUser(registerDTO);
                response.SetData(userToRegister);
            }
            else
            {
                response.SetStatus(false, $"User with {nameof(registerDTO.PrivateId).ToSpaced()} - '{registerDTO.PrivateId}' already exists");
            } 
        }
        else
        {
            response.SetStatus(false, $"User with {nameof(registerDTO.Email)} - '{registerDTO.Email}' already exists'");
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
            response.SetStatus(false, $"{validPasswordResponse.Message} for email: {userToAuthenticate.Email}");
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