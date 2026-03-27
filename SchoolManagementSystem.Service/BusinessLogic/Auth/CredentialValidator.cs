using ProjectHelperLibrary.Response;
using ProjectHelperLibrary.Utilities;
using SchoolManagementSystem.Data.Models;
using SchoolManagementSystem.Data.Repositories;
using SchoolManagementSystem.Service.DTOs.User.Auth;
using SchoolManagementSystem.Service.Mapping;

namespace SchoolManagementSystem.Service.BusinessLogic.Auth;

public class CredentialValidator
{
    private readonly UserRepository _userRepository = new();
    private readonly Mapper _mapper = new();
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
}