using ProjectHelperLibrary.Response;
using SchoolManagementSystem.Data.Models.JoinedModels;
using SchoolManagementSystem.Service.DTOs.User.Display;

namespace SchoolManagementSystem.Service.BusinessLogic;

public class UserService
{
    private readonly RepositoryFactory _repos;
    public UserService(RepositoryFactory repos)
    {
        _repos = repos;
    }
    
    #region User

    public async Task<DataResponse<UserDisplayDTO>> GetUserById(int id)
    {
        DataResponse<UserDisplayDTO> response = new();
        var userResponse = await _repos.UserRepository.GetById(id);
        
        if (userResponse.Success)
        {
            var userDisplay = await _repos.Mapper.UserToDisplayDTO(userResponse.Value);
            if (userDisplay is not null)
            {
                response.SetData(userDisplay);
            }
            else
            {
                response.SetStatus(false,"Could not map to display format");
            }
        }
        else
        {
            response.SetStatus(false, userResponse.Message);
        }

        return response;
    }
    
    #endregion
    
    #region Grade

    public async Task<DataResponse<decimal>> GetAverageSubjectGrade(int studentId, int subjectId)
    {
        DataResponse<decimal> response = new();
        var classesResponse = await _repos.SchoolClassRepository.GetClassesBySubjectId(subjectId);
        if (classesResponse.Success)
        {
            var schoolClass = classesResponse.Value[0];
            var subjectEnrollmentResponse =
                await _repos.SubjectEnrollmentRepository.GetByStudentAndClassIds(studentId, schoolClass.Id);
            if (subjectEnrollmentResponse.Success)
            {
                var assessmentsResponse =
                    await _repos.AssessmentRepository.GetBySubjectEnrollmentId(subjectEnrollmentResponse.Value.Id);
                if (assessmentsResponse.Success)
                {
                    var gradeValues = assessmentsResponse.Value.Select(assessment => assessment.GradeValue);
                    response.SetData(gradeValues.Average());
                }
                else
                {
                    response.SetStatus(false, assessmentsResponse.Message);
                }
            }
            else
            {
                response.SetStatus(false, subjectEnrollmentResponse.Message);
            }
        }
        else
        {
            response.SetStatus(false, classesResponse.Message);
        }

        return response;
    }

    public async Task<DataResponse<int>> GetFinalSubjectGrade(int studentId, int subjectId)
    {
        DataResponse<int> response = new();
        var averageGradeResponse = await GetAverageSubjectGrade(studentId, subjectId);
        if (averageGradeResponse.Success)
        {
            var finalGrade = (int)averageGradeResponse.Value;
            response.SetData(finalGrade);
        }
        else
        {
            response.SetStatus(false, averageGradeResponse.Message);
        }

        return response;
    }

    #endregion

    #region Subject

    public async Task<DataResponse<List<UserDisplayDTO>>> GetAllStudentsWithSubject(int subjectId)
    {
        var response = new DataResponse<List<UserDisplayDTO>>();
        
        var classesResponse = await _repos.SchoolClassRepository.GetClassesBySubjectId(subjectId);

        if (classesResponse.Success)
        {
            var classIds = classesResponse.Value.Select(c => c.Id).ToList();
            var subjectEnrollments = await _repos.SubjectEnrollmentRepository.GetByClassIds(classIds);
            
            if (subjectEnrollments.Success)
            {
                var studentsResponse = await _repos.UserRepository.GetStudentsBySubjectEnrollments(subjectEnrollments.Value);
                
                if (studentsResponse.Success)
                {
                    var dtosResponse = await _repos.Mapper.UsersToDisplayDTOs(studentsResponse.Value);
                    
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
                    response.SetStatus(false, studentsResponse.Message);
                }
            }
            else
            {
                response.SetStatus(false, subjectEnrollments.Message);
            }
        }
        else
        {
            response.SetStatus(false, classesResponse.Message);
        }

        return response;
    }

    #endregion

    #region SubjectEnrollment

    private async Task<DataResponse<SubjectEnrollment>> GetSubjectEnrollment(int studentId, int subjectId)
    {
        DataResponse<SubjectEnrollment> response = new();
        var classesResponse = await _repos.SchoolClassRepository.GetClassesBySubjectId(subjectId);
        if (classesResponse.Success)
        {
            var schoolClass = classesResponse.Value[0];
            var subjectEnrollmentResponse =
                await _repos.SubjectEnrollmentRepository.GetByStudentAndClassIds(studentId, schoolClass.Id);
        }
        
        return response;
    }

    #endregion
    
    #region Test

    public async Task<DataResponse<UserDisplayDTO>> GetUserByEmail(string email)
    {
        DataResponse<UserDisplayDTO> response = new();
        var userByEmailResponse = await _repos.UserRepository.GetByEmail(email);

        if (userByEmailResponse.Success)
        {
            var dto = await _repos.Mapper.UserToDisplayDTO(userByEmailResponse.Value);
            if (dto is not null)
            {
                response.SetData(dto);
            }
            else
            {
                response.SetStatus(false, "Could not map to display dto");
            }
        }
        else
        {
            response.SetStatus(false, userByEmailResponse.Message);
        }

        return response;
    }
    
    #endregion
}