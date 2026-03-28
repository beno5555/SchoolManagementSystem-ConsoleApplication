using ProjectHelperLibrary.Response;
using SchoolManagementSystem.Data.Models;
using SchoolManagementSystem.Service.BusinessLogic.Factories;
using SchoolManagementSystem.Service.DTOs.StudentJournal;
using SchoolManagementSystem.Service.DTOs.User.Auth;
using SchoolManagementSystem.Service.DTOs.User.Display;

namespace SchoolManagementSystem.Service.BusinessLogic.Utilities;

public class MapperService
{
    // private readonly RoleRepository _roleRepository = new();
    // private readonly GroupRepository _groupRepository = new();
    // private readonly RoomRepository _roomRepository = new();
    // private readonly PasswordHasher _passwordHasher = new();

    private readonly RepositoryFactory _repos;
    private readonly PasswordHasher _passwordHasher;
    private readonly MethodHelper _helper;

    public MapperService(RepositoryFactory repos, MethodHelper helper, PasswordHasher passwordHasher)
    {
        _repos = repos;
        _helper = helper;
        _passwordHasher = passwordHasher;
    }   
    
    #region Singles
    
    #region Users

    public async Task<DataResponse<UserDisplayDTO>> UserToDisplayDTO(User user)
    {
        var response = new DataResponse<UserDisplayDTO>();

        string? groupName = null;
        string? officeRoomName = null;

        if (user.GroupId.HasValue)
        {
            var groupResponse = await _repos.GroupRepository.GetById(user.GroupId.Value);
            groupName = groupResponse.Success ? groupResponse.Value.Name : null;
        }

        if (user.OfficeRoomId.HasValue)
        {
            var officeRoomResponse = await _repos.RoomRepository.GetById(user.OfficeRoomId.Value);
            officeRoomName = officeRoomResponse.Success ? officeRoomResponse.Value.Name : null;
        }
        
        var roleResponse = await _repos.RoleRepository.GetById(user.RoleId);
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
            response.SetData(dto);
        }
        else
        {
            response.SetStatus(false, "Mapping to display format failed");
        }
        return response;
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

        if (users.Count > 0)
        {
            var tasks = users.Select(UserToDisplayDTO);

            var dtosResponse = await _helper.TasksToValues(tasks);
            
            if (dtosResponse.Success)
            {
                response.SetData(dtosResponse.Value);
            }
            else
            {
                response.SetStatus(false, dtosResponse.Message); 
            }
        }
        else
        {
            response.SetStatus(false, "List is empty");
        }
        return response;
    }
    #endregion

    public async Task<DataResponse<Assessment>> ToAssessment(AssessmentDTO assessmentDTO, int subjectEnrollmentId)
    {
        var response = new DataResponse<Assessment>();

        var assignmentExists = await _repos.AssignmentRepository.ExistsAsync(assessmentDTO.AssignmentId);
        if (assignmentExists)
        {
            var assessment = new Assessment
            {
                GradeValue = assessmentDTO.NewGrade,
                AssignmentId = assessmentDTO.AssignmentId,
                Comment = assessmentDTO.Comment,
                
                SubjectEnrollmentId = subjectEnrollmentId, 
            };
            response.SetData(assessment);
        }
        else
        {
            response.SetStatus(false, "Assignment not found");
        }

        return response;
    }
}