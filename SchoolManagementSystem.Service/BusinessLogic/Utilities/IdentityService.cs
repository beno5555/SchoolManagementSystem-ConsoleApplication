using ProjectHelperLibrary.Response;
using SchoolManagementSystem.Data.Models;
using SchoolManagementSystem.Service.BusinessLogic.Factories;
using SchoolManagementSystem.Service.DTOs.User.Auth;
using SchoolManagementSystem.Service.DTOs.User.Display;

namespace SchoolManagementSystem.Service.BusinessLogic.Utilities;

public class IdentityService
{
    
    private readonly RepositoryFactory _repos;
    
    // utils
    private readonly MapperService _mapperService;
    private readonly PasswordHasher _passwordHasher;
    
    public IdentityService(RepositoryFactory repos, MapperService mapperService, PasswordHasher passwordHasher)
    {
        _repos = repos;
        _mapperService = mapperService;
        _passwordHasher = passwordHasher;
    }
    
    #region Methods
    
    public async Task<DataResponse<User>> PrepareForRegistration(BaseRegisterDTO registerDTO)
    {
        DataResponse<User> response = new();
        var credentialValidationResponse = await AreCredentialsUnique(registerDTO.Email, registerDTO.PrivateId);

        if (credentialValidationResponse.Success)
        {
            var userToRegister = _mapperService.RegisterDTOToUser(registerDTO);
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
        bool existsByEmail = await _repos.UserRepository.ExistsByEmail(email);
        if (existsByEmail)
        {
            response.SetStatus(false, "Email is already taken");
        }
        else
        {
            bool existsByPrivateId = await _repos.UserRepository.ExistsByPrivateId(privateId);
            if (existsByPrivateId)
            {
                response.SetStatus(false, "User with private id is already in the database");
            }
        }

        return response;
    }

    public async Task<DataResponse<UserDisplayDTO>> Authenticate(User userToAuthenticate, string password)
    {
        DataResponse<UserDisplayDTO> response = new();

        var validPasswordResponse = _passwordHasher.VerifyPassword(
            password, 
            userToAuthenticate.PasswordHash, 
            userToAuthenticate.PasswordSalt);
                
        if (validPasswordResponse.Success)
        {
            response = await _mapperService.UserToDisplayDTO(userToAuthenticate);
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
            response = await _repos.UserRepository.GetByEmail(loginDTO.Email);
        }
        else if (!string.IsNullOrWhiteSpace(loginDTO.PrivateId))
        {
            response = await _repos.UserRepository.GetByPrivateId(loginDTO.PrivateId);
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