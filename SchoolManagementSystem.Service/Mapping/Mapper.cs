using ProjectHelperLibrary.Response;
using SchoolManagementSystem.Data.Models;
using SchoolManagementSystem.Data.Repositories;
using SchoolManagementSystem.Service.BusinessLogic.Auth;
using SchoolManagementSystem.Service.DTOs.User.Auth;
using SchoolManagementSystem.Service.DTOs.User.Display;

namespace SchoolManagementSystem.Service.Mapping;

public class Mapper
{
    private readonly RoleRepository _roleRepository = new();
    private readonly GroupRepository _groupRepository = new();
    private readonly RoomRepository _roomRepository = new();
    private readonly PasswordHasher _passwordHasher = new();
    
    #region Singles
    
    #region Users
    public async Task<UserDisplayDTO?> UserToDisplayDTO(User user)
    {
        UserDisplayDTO? result = null;

        string? groupName = null;
        string? officeRoomName = null;

        if (user.GroupId.HasValue)
        {
            var groupResponse = await _groupRepository.GetById(user.GroupId.Value);
            groupName = groupResponse.Success ? groupResponse.Value.Name : null;
        }

        if (user.OfficeRoomId.HasValue)
        {
            var officeRoomResponse = await _roomRepository.GetById(user.OfficeRoomId.Value);
            officeRoomName = officeRoomResponse.Success ? officeRoomResponse.Value.Name : null;
        }
        
        var roleResponse = await _roleRepository.GetById(user.RoleId);
        if (roleResponse.Success)
        {
            var dto = new UserDisplayDTO
            {
                FullName = user.FullName,
                Email = user.Email,
                PrivateId =  user.PrivateId,
                RegisterDate =  user.RegistrationDate,
                DateOfBirth = user.DateOfBirth,
                FinalGrade = user.FinalGrade,
                
                RoleName = roleResponse.Value.Name,
                GroupName = groupName,
                OfficeRoomName = officeRoomName
            };
            result = dto;
        }
        return result;
    }

    public User RegisterDTOToUser(BaseRegisterDTO registerDTO)
    {
        var (hash, salt) = _passwordHasher.HashPassword(registerDTO.Password);

        var result = new User
        {
            FirstName =  registerDTO.FirstName,
            LastName =  registerDTO.LastName,
            Email =  registerDTO.Email,
            PhoneNumber =  registerDTO.PhoneNumber,
            PasswordHash = hash,
            PasswordSalt = salt,
            Address = registerDTO.Address,
            PrivateId =  registerDTO.PrivateId,
            
            DateOfBirth = registerDTO.DateOfBirth,
            RegistrationDate = registerDTO.RegistrationDate,
        };
        
        return result;
    }
    
    #endregion
    
    #endregion
    
    #region Collections
    public async Task<DataResponse<List<UserDisplayDTO>>> UsersToDisplayDTOs(List<User> users)
    {
        DataResponse<List<UserDisplayDTO>> response = new();

        var tasks = users.Select(UserToDisplayDTO);
        var mappingResult = await Task.WhenAll(tasks);
        
        List<UserDisplayDTO> userDisplayDTOs = mappingResult.OfType<UserDisplayDTO>().ToList();
        if (userDisplayDTOs.Any())
        {
            if (userDisplayDTOs.Count == users.Count)
            {
                response.SetData(userDisplayDTOs);
            }
            else
            {
                response.SetStatus(false, "One or more users could not be mapped correctly"); 
            }
        }
        else
        {
            response.SetStatus(false, "List is empty");
        }
        
        return response;
    }
    #endregion
}