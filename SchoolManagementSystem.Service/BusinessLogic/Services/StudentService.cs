using System.Numerics;
using ProjectHelperLibrary.Response;
using SchoolManagementSystem.Data.Models;
using SchoolManagementSystem.Data.Models.JoinedModels;
using SchoolManagementSystem.Service.BusinessLogic.Factories;
using SchoolManagementSystem.Service.DTOs.StudentJournal;
using SchoolManagementSystem.Service.DTOs.User.Display;

namespace SchoolManagementSystem.Service.BusinessLogic.Services;

public class StudentService : BaseService
{
    private readonly UtilityFactory _utilities;
    public StudentService(UtilityFactory utilities)
    {
        _utilities = utilities;
    }
    
    #region Entry points

    #region Grade
    public async Task<DataResponse<decimal>> GetAverageSubjectGrade(int studentId, int subjectId)
    {
        return await GetSubjectGrade(studentId, subjectId, enrollment => enrollment.AverageGrade);
    }
    
    public async Task<DataResponse<int>> GetFinalSubjectGrade(int studentId, int subjectId)
    {
        return await GetSubjectGrade(studentId, subjectId, enrollment => enrollment.FinalGrade);
    }

    public async Task<DataResponse<T>> GetSubjectGrade<T>(int studentId, int subjectId, Func<SubjectEnrollment, T> getGrade) where T : INumber<T>
    {
        return await Execute<DataResponse<T>, DataResponse<SubjectEnrollment>>(async response =>
        {
            var subjectEnrollmentResponse = await GetSubjectEnrollment(studentId, subjectId);
            if (subjectEnrollmentResponse.Success)
            {
                var grade = getGrade(subjectEnrollmentResponse.Value);
                response.SetData(grade);
            }

            return subjectEnrollmentResponse;
        });
    }

    #endregion

    #region New Assessment
    public async Task<BaseResponse> AssessStudent(int studentId, int subjectId, AssessmentDTO assessmentDTO)
    {
        var response = new BaseResponse();

        var subjectEnrollmentResponse = await GetSubjectEnrollment(studentId, subjectId);
        
        if (subjectEnrollmentResponse.Success)
        {
            var subjectEnrollment = subjectEnrollmentResponse.Value;
            response = await CreateAssessment(subjectEnrollment, assessmentDTO);
        }
        else
        {
            response.SetStatus(false, subjectEnrollmentResponse.Message);
        }
        
        return response;
    }
    
    #endregion

    private async Task<BaseResponse> CreateAssessment(SubjectEnrollment subjectEnrollment, AssessmentDTO assessmentDTO)
    {
        var response = new BaseResponse();
        var toAssessmentResponse = await _utilities.MapperService.ToAssessment(assessmentDTO, subjectEnrollment.Id);
            
        if (toAssessmentResponse.Success)
        {
            response = await ApplyAssessment(subjectEnrollment, toAssessmentResponse.Value);
        }
        else
        {
            response.SetStatus(false, toAssessmentResponse.Message);
        }

        return response;
    }

    private async Task<BaseResponse> ApplyAssessment(SubjectEnrollment subjectEnrollment, Assessment newAssessment)
    {
        return await Execute<BaseResponse, BaseResponse>(async _ =>
        {
            var updateResponse = await UpdateSubjectGrade(subjectEnrollment, newAssessment.GradeValue);
            if (updateResponse.Success)
            {
                await _utilities.Repos.AssessmentRepository.AddAsync(newAssessment);
            }

            return updateResponse;
        });
    }

    public async Task<BaseResponse> UpdateSubjectGrade(SubjectEnrollment subjectEnrollment, decimal newGrade)
    {
        var response = new BaseResponse();
        var assessmentsResponse =
            await _utilities.Repos.AssessmentRepository.GetBySubjectEnrollmentId(subjectEnrollment.Id);
        
        if (assessmentsResponse.Success)
        {
            decimal newAverage = GetNewAverage(assessmentsResponse.Value, newGrade);
            subjectEnrollment.AverageGrade = newAverage;
            
            response = await _utilities.Repos.SubjectEnrollmentRepository
                .UpdateAsync(subjectEnrollment);
        }
        else
        {
            response.SetStatus(false, assessmentsResponse.Message);
        }

        return response;
    }

    private decimal GetNewAverage(List<Assessment> assessments, decimal newGrade)
    {
        int assessmentCount = assessments.Count;
        decimal previousGradesSum = assessments.Sum(assessment => assessment.GradeValue);
        decimal newAverageGrade = (previousGradesSum + newGrade) / (assessmentCount + 1);
        
        return newAverageGrade;
    }

    #endregion
    
    #region Students
    
    public async Task<DataResponse<List<UserDisplayDTO>>> GetAllStudentsWithSubject(int subjectId)
    {
        var response = new DataResponse<List<UserDisplayDTO>>();

        var studentsResponse = await GetStudentsBySubject(subjectId);
        
        if (studentsResponse.Success)
        {
            response = await _utilities.MapperService.UsersToDisplayDTOs(studentsResponse.Value);
        }
        else
        {
            response.SetStatus(false, studentsResponse.Message);
        }
        return response;
    }
    
    #endregion
    

    #region Data Access Through multiple repositories (Needed because models do not have direct references, only Ids)
    
    #region Student

    public async Task<DataResponse<List<User>>> GetStudentsBySubject(int subjectId)
    {
        var response =  new DataResponse<List<User>>();
        var subjectEnrollments = await GetSubjectEnrollmentsBySubject(subjectId);
        if (subjectEnrollments.Success)
        {
            response =
                await _utilities.Repos.UserRepository.GetStudentsBySubjectEnrollments(subjectEnrollments.Value);
        }
        else
        {
            response.SetStatus(false, subjectEnrollments.Message);
        }

        return response;
    }
    
    #endregion
    
    #region SubjectEnrollment

    public async Task<DataResponse<SubjectEnrollment>> GetSubjectEnrollment(int studentId, int subjectId)
    {
        DataResponse<SubjectEnrollment> response = new();
        var classResponse = await _utilities.Repos.SchoolClassRepository.GetClassBySubjectId(subjectId);
        if (classResponse.Success)
        {
            response = await _utilities.Repos.SubjectEnrollmentRepository.GetByStudentAndClassIds(studentId, classResponse.Value.Id);
        }
        else
        {
            response.SetStatus(false, classResponse.Message);
        }

        return response;
    }

    public async Task<DataResponse<List<SubjectEnrollment>>> GetSubjectEnrollmentsBySubject(int subjectId)
    {
        var response = new  DataResponse<List<SubjectEnrollment>>();
        var classIdsResponse = await GetClassIdsBySubject(subjectId);
        if (classIdsResponse.Success)
        {
            var subjectEnrollmentsResponse = await _utilities.Repos.SubjectEnrollmentRepository.GetByClassIds(classIdsResponse.Value);
            if (subjectEnrollmentsResponse.Success)
            {
                response.SetData(subjectEnrollmentsResponse.Value);
            }
            else
            {
                response.SetStatus(false, subjectEnrollmentsResponse.Message);
            }
        }
        else
        {
            response.SetStatus(false, classIdsResponse.Message);
        }

        return response;
    }

    #endregion
    
    #region Assessments

    public async Task<DataResponse<List<Assessment>>> GetAssessments(int studentId, int subjectId)
    {
        DataResponse<List<Assessment>> response = new();
        var subjectEnrollmentResponse = await GetSubjectEnrollment(studentId, subjectId);
        if (subjectEnrollmentResponse.Success)
        {
            var subjectEnrollmentId = subjectEnrollmentResponse.Value.Id;
            response =
                await _utilities.Repos.AssessmentRepository.GetBySubjectEnrollmentId(subjectEnrollmentId);
        }
        else
        {
            response.SetStatus(false, subjectEnrollmentResponse.Message);
        }

        return response;
    }

    
    #endregion 

    #region Classes

    private async Task<DataResponse<List<int>>> GetClassIdsBySubject(int subjectId)
    {
        return await Execute<DataResponse<List<int>>, DataResponse<List<SchoolClass>>>(async response =>
        {
            var classesResponse = await _utilities.Repos.SchoolClassRepository.GetClassesBySubjectId(subjectId);

            if (classesResponse.Success)
            {
                var classIds = classesResponse.Value.Select(c => c.Id).ToList();
                response.SetData(classIds);
            }

            return classesResponse;
        });
    }
    
    #endregion
    
    #region Grades
    
    public decimal GetAverageGrade(List<Assessment> assessments)
    {
        decimal averageGrade = 
            assessments.Count > 0 
                ? assessments.Average(a => a.GradeValue) 
                : 0;
        return averageGrade;
    }
    
    #endregion
    #endregion
    
    #region Test

    public async Task<DataResponse<UserDisplayDTO>> GetUserByEmail(string email)
    {
        DataResponse<UserDisplayDTO> response = new();
        var userByEmailResponse = await _utilities.Repos.UserRepository.GetByEmail(email);

        if (userByEmailResponse.Success)
        { 
            response = await _utilities.MapperService.UserToDisplayDTO(userByEmailResponse.Value);
        }
        else
        {
            response.SetStatus(false, userByEmailResponse.Message);
        }

        return response;
    }
    
    #endregion
    
    // does not belong here    
    #region User


    public async Task<DataResponse<UserDisplayDTO>> GetUserById(int id)
    {
        DataResponse<UserDisplayDTO> response = new();
        var userResponse = await _utilities.Repos.UserRepository.GetById(id);
        
        if (userResponse.Success)
        {
            response = await _utilities.MapperService.UserToDisplayDTO(userResponse.Value);
        }
        else
        {
            response.SetStatus(false, userResponse.Message);
        }

        return response;
    }
    #endregion
}