using System.Numerics;
using ProjectHelperLibrary.Response;
using SchoolManagementSystem.Data.Models;
using SchoolManagementSystem.Data.Models.JoinedModels;
using SchoolManagementSystem.Data.Models.Named;
using SchoolManagementSystem.Service.BusinessLogic.Factories;
using SchoolManagementSystem.Service.DTOs.Academic.Assessments;
using SchoolManagementSystem.Service.DTOs.Academic.Assignments;
using SchoolManagementSystem.Service.DTOs.Academic.Submissions;
using SchoolManagementSystem.Service.DTOs.User.Display;

namespace SchoolManagementSystem.Service.BusinessLogic.Services;

public class StudentService
{
    private readonly UtilityFactory _utilities;
    public StudentService(UtilityFactory utilities)
    {
        _utilities = utilities;
    }
    
    #region Entry points

    #region Grade
    
    #region Single Subject
    public async Task<DataResponse<decimal>> GetAverageSubjectGrade(int studentId, int subjectId)
    {
        return await GetSubjectGrade(studentId, subjectId, enrollment => enrollment.AverageGrade);
    }
    
    public async Task<DataResponse<int>> GetFinalSubjectGrade(int studentId, int subjectId)
    {
        return await GetSubjectGrade(studentId, subjectId, enrollment => enrollment.FinalGrade);
    }
    #endregion

    #region All Subjects
    public async Task<DataResponse<List<decimal>>> GetAllAverageSubjectGrades(int studentId)
    {
        return await GetAllSubjectGrades(studentId, enrollment => enrollment.AverageGrade);
    }

    public async Task<DataResponse<List<int>>> GetAllFinalSubjectGrades(int studentId)
    {
        return await GetAllSubjectGrades(studentId, enrollment => enrollment.FinalGrade);
    }
    #endregion

    #region Student Average & Final
    public async Task<DataResponse<decimal>> GetAverageStudentGrade(int studentId)
    {
        return await GetStudentGrade(studentId, student => student.AverageGrade);
    }

    public async Task<DataResponse<int>> GetFinalStudentGrade(int studentId)
    {
        return await GetStudentGrade(studentId, student => student.FinalGrade);
    }
    #endregion
    
    #region Core Grade Methods
    public async Task<DataResponse<List<T>>> GetAllSubjectGrades<T>(int studentId, Func<SubjectEnrollment, T> getGrade)
        where T : struct, INumber<T>
    {
        return await _utilities.MethodHelper.Execute<DataResponse<List<T>>>(async response =>
        {
            var subjectsResponse = await GetSubjectsByStudent(studentId);

            if (subjectsResponse.Success)
            {
                var subjectIds = await _utilities.Repos.SubjectRepository.GetIds(subjectsResponse.Value);
                
                var tasks = subjectIds.Select(id => GetSubjectGrade(studentId, id, getGrade));
                var gradesResponse = await _utilities.MethodHelper.TasksToValues(tasks);

                if (gradesResponse.Success)
                {
                    response.SetData(gradesResponse.Value);
                }
                else
                {
                    response.SetStatus(false, gradesResponse.Message);
                }
            }
        });
    }

    private async Task<DataResponse<T>> GetSubjectGrade<T>(int studentId, int subjectId, Func<SubjectEnrollment, T> getGrade) where T : INumber<T>
    {
        return await _utilities.MethodHelper.Execute<DataResponse<T>, DataResponse<SubjectEnrollment>>(async response =>
        {
            var subjectEnrollmentResponse = await _utilities.AcademicService.GetSubjectEnrollment(studentId, subjectId);
            if (subjectEnrollmentResponse.Success)
            {
                var grade = getGrade(subjectEnrollmentResponse.Value);
                response.SetData(grade);
            }

            return subjectEnrollmentResponse;
        });
    }
    
    private async Task<DataResponse<T>> GetStudentGrade<T>(int studentId, Func<User, T?> getGrade) where T : struct, INumber<T>
    {
        return await _utilities.MethodHelper.Execute<DataResponse<T>, DataResponse<User>>(async response =>
        {
            var studentResponse = await _utilities.Repos.UserRepository.GetById(studentId);
            if (studentResponse.Success)
            {
                var grade = getGrade(studentResponse.Value);
                if (grade is not null)
                {
                    response.SetData(grade.Value);
                }
                else
                {
                    response.SetStatus(false, "Grade is null (should not be seen)");
                }
            }
            return studentResponse;
        });
    }
    #endregion
    
    #endregion

    #region New Assessment, Submissions, Assignments...
    public async Task<BaseResponse> AssessStudent(int studentId, int subjectId, AssessmentDTO assessmentDTO)
    {
        var response = new BaseResponse();

        var subjectEnrollmentResponse = await _utilities.AcademicService.GetSubjectEnrollment(studentId, subjectId);
        if (subjectEnrollmentResponse.Success)
        {
            var subjectEnrollment = subjectEnrollmentResponse.Value;
            response = await _utilities.AcademicService.CreateAssessment(subjectEnrollment, assessmentDTO);
        }
        else
        {
            response.SetStatus(false, subjectEnrollmentResponse.Message);
        }
        
        return response;
    }

    public async Task<BaseResponse> SubmitWork(int studentId, int subjectId, SubmissionDTO submissionDTO)
    {
        var response = new BaseResponse();
        var subjectEnrollmentResponse = await _utilities.AcademicService.GetSubjectEnrollment(studentId, subjectId);

        if (subjectEnrollmentResponse.Success)
        {
            response = await _utilities.AcademicService.CreateSubmission(subjectEnrollmentResponse.Value, submissionDTO);
        }

        return response;
    }

    public async Task<BaseResponse> UploadAssignment(AssignmentUploadDTO assignmentUploadDTO, int teacherId)
    {
        var response = new BaseResponse();

        var groupClassResponse =
            await GetGroupClass(assignmentUploadDTO.GroupId, assignmentUploadDTO.SubjectId, teacherId);

        if (groupClassResponse.Success)
        {
            response = await _utilities.AcademicService.UploadAssignmentToGroupClass(assignmentUploadDTO, groupClassResponse.Value.Id);
        }
        else
        {
            response.SetStatus(false, groupClassResponse.Message);
        }
        
        return response;
    }

    private async Task<DataResponse<GroupClass>> GetGroupClass(int groupId, int subjectId, int teacherId)
    {
        var response = new DataResponse<GroupClass>();
        var validIds = await _utilities.AcademicService.AreValidIds(groupId, subjectId);
        if (validIds.Success)
        {
            var classResponse = await _utilities.Repos.SchoolClassRepository.GetBySubjectAndTeacherId(subjectId, teacherId);

            if (classResponse.Success)
            {
                response = await _utilities.Repos.GroupClassRepository.GetByGroupAndClassIdAsync(groupId, classResponse.Value.Id);
            }
            else
            {
                response.SetStatus(false, classResponse.Message);
            }
        }
        else
        {
            response.SetStatus(false, validIds.Message);
        }

        return response;
    }

    public async Task<DataResponse<List<AssignmentType>>> GetAssignmentTypes()
    {
        return await _utilities.Repos.AssignmentTypeRepository.GetAll();
    }

    #endregion
    
    #region Subjects
    
    public async Task<DataResponse<List<Subject>>> GetSubjectsByStudent(int studentId)
    {
        var response = new DataResponse<List<Subject>>();

        var subjectEnrollmentsResponse = await _utilities.AcademicService.GetSubjectEnrollments(studentId);

        if (subjectEnrollmentsResponse.Success)
        {
            var subjectEnrollmentIds = await _utilities.Repos.SubjectEnrollmentRepository.GetIds(subjectEnrollmentsResponse.Value);
            var subjectsResponse = await _utilities.Repos.SubjectRepository.GetBySubjectEnrollmentIds(subjectEnrollmentIds);
            
            if (subjectsResponse.Success)
            {
                response.SetData(subjectsResponse.Value);    
            }
            else
            {
                response.SetStatus(false, subjectsResponse.Message);
            }
        }
        else
        {
            response.SetStatus(false, subjectEnrollmentsResponse.Message);
        }

        return response;
    }

    public async Task<DataResponse<List<Subject>>> GetSubjectsByTeacher(int teacherId)
    {
        var response = new DataResponse<List<Subject>>();
        var classesResponse = await _utilities.Repos.SchoolClassRepository.GetByTeacherId(teacherId);

        if (classesResponse.Success)
        {
            var subjectIds = classesResponse.Value.Select(c => c.SubjectId).Distinct().ToList();
            response = await _utilities.Repos.SubjectRepository.GetByIds(subjectIds);
        }

        return response;
    }

    public async Task<DataResponse<List<Subject>>> GetSubjectsNotAssignedToTeacher(int teacherId)
    {
        var response = new DataResponse<List<Subject>>();

        var classesResponse = await _utilities.Repos.SchoolClassRepository.GetByTeacherId(teacherId);
        var subjectIds = classesResponse.Value.Select(c => c.SubjectId).Distinct().ToList();

        response = await _utilities.Repos.SubjectRepository.GetByNotIds(subjectIds);
        return response;
    }
    
    #endregion
    
    #region Groups

    public async Task<DataResponse<List<Group>>> GetGroupsBySubjectAndTeacher(int subjectId, int teacherId)
    {
        var response = new DataResponse<List<Group>>();
        var classResponse = await _utilities.Repos.SchoolClassRepository.GetBySubjectAndTeacherId(subjectId, teacherId);

        if (classResponse.Success)
        {
            var groupClassesResponse =
                await _utilities.Repos.GroupClassRepository.GetByClassIdAsync(classResponse.Value.Id);

            if (groupClassesResponse.Success)
            {
                var groupClasses = groupClassesResponse.Value;
                var groupIds = groupClasses.Select(gc => gc.GroupId).Distinct().ToList();
                response = await _utilities.Repos.GroupRepository.GetByIds(groupIds);
            }
            else
            {
                response.SetStatus(false, "Could not get subjects --> " + groupClassesResponse.Message);
            }
        }
        else
        {
            response.SetStatus(false, "Could not get subjects --> " + classResponse.Message);
        }
        
        return response;
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

    public async Task<DataResponse<UserDisplayDTO>> GetStudentById(int id)
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


    public async Task<BaseResponse> AssignSubjectToTeacher(int subjectId, int teacherId)
    {
        var response = new BaseResponse();
        var schoolClassExists = await _utilities.Repos.SchoolClassRepository.ExistsByTeacherAndSubjectIds(subjectId, teacherId);

        if (!schoolClassExists)
        {
            var schoolClass = new SchoolClass
            {
                SubjectId = subjectId,
                TeacherId = teacherId
            };
            await _utilities.Repos.SchoolClassRepository.AddAsync(schoolClass);
        }
        else
        {
            response.SetStatus(false, "Subject is already assigned to the teacher");
        }

        return response;
    }

    public async Task<DataResponse<List<string>>> GetTeacherSubjectNames(List<SchoolClass> classes)
    {
        await Task.Delay(3000);
        return null;
    }
}