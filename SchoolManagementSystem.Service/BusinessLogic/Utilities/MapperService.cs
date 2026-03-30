using ProjectHelperLibrary.Response;
using SchoolManagementSystem.Data.Models;
using SchoolManagementSystem.Data.Models.Academic;
using SchoolManagementSystem.Service.DTOs.Academic.Assessments;
using SchoolManagementSystem.Service.DTOs.User.Auth;
using SchoolManagementSystem.Service.DTOs.User.Display;
using SchoolManagementSystem.Service.BusinessLogic.Factories;
using SchoolManagementSystem.Service.DTOs.Academic.Assignments;
using SchoolManagementSystem.Service.DTOs.Academic.Submissions;

namespace SchoolManagementSystem.Service.BusinessLogic.Utilities;

public class MapperService
{
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

    public async Task<DataResponse<SessionUser?>> UserToSessionDTO(User user)
    {
        var response = new DataResponse<SessionUser?>();
        var roleResponse = await _repos.RoleRepository.GetById(user.RoleId);
        if (roleResponse.Success)
        {
            var sessionUser = new SessionUser
            {
                Id = user.Id,
                FullName = user.FullName,
                Email = user.Email,
                Role = roleResponse.Value.Name,
                
            };
            response.SetData(sessionUser);
        }
        else
        {
            response.SetStatus(false, roleResponse.Message);
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

    public async Task<DataResponse<Assessment>> ToAssessment(AssessmentDTO assessmentDTO, Submission submission)
    {
        var response = new DataResponse<Assessment>();
        var assessment = new Assessment
        {
            GradeValue = assessmentDTO.NewGradeValue,
            VerbalAssessment = assessmentDTO.VerbalAssessment,
            
            SubmissionId = assessmentDTO.SubmissionId,
            AssignmentId = submission.AssignmentId
        };
        response.SetData(assessment);

        return response;
    }

    public async Task<DataResponse<Submission>> ToSubmission(SubmissionDTO submissionDTO, int subjectEnrollmentId)
    {
        var response = new DataResponse<Submission>();
        var assignmentExists = await _repos.AssignmentRepository.ExistsAsync(submissionDTO.AssignmentId);

        if (assignmentExists)
        {
            var submission = new Submission
            {
                SubmissionContent = submissionDTO.SubmissionContent,
                StudentComment = submissionDTO.StudentComment,
                AssignmentId =  submissionDTO.AssignmentId,
                SubjectEnrollmentId = subjectEnrollmentId
            };
            response.SetData(submission);
        }
        else
        {
            response.SetStatus(false, "Assignment not found");
        }
        
        return response;
    }

    public DataResponse<Assignment> ToAssignment(AssignmentUploadDTO assignmentUploadDTO, int groupClassId)
    {
        var response = new DataResponse<Assignment>();

        var assignment = new Assignment
        {
            AssignmentName = assignmentUploadDTO.AssignmentName,
            Description = assignmentUploadDTO.Description,
            AssignmentTypeId =  assignmentUploadDTO.AssignmentTypeId,
            GroupClassId = groupClassId,
            DueDate = assignmentUploadDTO.DueDate,
        };
        response.SetData(assignment);
        return response;
    }
}