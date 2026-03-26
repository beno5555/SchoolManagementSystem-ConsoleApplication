using ProjectHelperLibrary.Response;
using SchoolManagementSystem.Data.Models.JoinedModels;
using SchoolManagementSystem.Data.Repositories;
using SchoolManagementSystem.Service.DTOs.User.Display;
using SchoolManagementSystem.Service.Mapping;

namespace SchoolManagementSystem.Service.BusinessLogic;

public class UserService
{
    private readonly UserRepository _userRepository = new();
    private readonly SubjectRepository _subjectRepository = new();
    private readonly RoleRepository _roleRepository = new();
    private readonly SubjectEnrollmentRepository _subjectEnrollmentRepository = new();
    private readonly SchoolClassRepository _schoolClassRepository = new();
    private readonly AssessmentRepository _assessmentRepository = new();
    private readonly Mapper _mapper = new();
    
    #region User

    public async Task<DataResponse<UserDisplayDto>> GetUserById(int id)
    {
        DataResponse<UserDisplayDto> response = new();
        var userResponse = await _userRepository.GetById(id);
        
        if (userResponse.Success)
        {
            var userDisplay = await _mapper.UserToDisplayDTO(userResponse.Value);
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
        var classesResponse = await _schoolClassRepository.GetClassesBySubjectId(subjectId);
        if (classesResponse.Success)
        {
            var schoolClass = classesResponse.Value[0];
            var subjectEnrollmentResponse =
                await _subjectEnrollmentRepository.GetByStudentAndClassIds(studentId, schoolClass.Id);
            if (subjectEnrollmentResponse.Success)
            {
                var assessmentsResponse =
                    await _assessmentRepository.GetBySubjectEnrollmentId(subjectEnrollmentResponse.Value.Id);
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

    public async Task<DataResponse<List<UserDisplayDto>>> GetAllStudentsWithSubject(int subjectId)
    {
        var response = new DataResponse<List<UserDisplayDto>>();
        var classesResponse = await _schoolClassRepository.GetClassesBySubjectId(subjectId);

        if (classesResponse.Success)
        {
            var classIds = classesResponse.Value.Select(c => c.Id).ToList();
            var subjectEnrollments = await _subjectEnrollmentRepository.GetByClassIds(classIds);
            
            if (subjectEnrollments.Success)
            {
                var studentsResponse = await _userRepository.GetStudentsBySubjectEnrollments(subjectEnrollments.Value);
                
                if (studentsResponse.Success)
                {
                    var dtosResponse = await _mapper.UsersToDisplayDTOs(studentsResponse.Value);
                    
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
        var classesResponse = await _schoolClassRepository.GetClassesBySubjectId(subjectId);
        if (classesResponse.Success)
        {
            var schoolClass = classesResponse.Value[0];
            var subjectEnrollmentResponse =
                await _subjectEnrollmentRepository.GetByStudentAndClassIds(studentId, schoolClass.Id);
        }
        
        return response;
    }

    #endregion
}