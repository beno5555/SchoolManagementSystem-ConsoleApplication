using ProjectHelperLibrary.Response;
using SchoolManagementSystem.Data.Repositories;
using SchoolManagementSystem.Service.DTOs.User.Auth;
using SchoolManagementSystem.Service.Mapping;

namespace SchoolManagementSystem.Service.BusinessLogic;

public class AuthService
{
    private readonly UserRepository _userRepository = new();
    private readonly Mapper _mapper = new();

    public async Task<BaseResponse> RegisterUser(RegisterDTO userToRegister)
    {
        BaseResponse response = new();
        
        var user = await _mapper.RegisterDTOToUser(userToRegister);
        if (user is not null)
        {
            user.GroupId = 5;
            await _userRepository.AddAsync(user);
            await _userRepository.SaveAsync();
        }
        else
        {
            response.SetStatus(false, "Invalid user credentials");
        }
        return response;
    } 
}