using ProjectHelperLibrary.Response;
using SchoolManagementSystem.Data.Models;
using SchoolManagementSystem.Data.Models.Academic;
using SchoolManagementSystem.Data.Models.Named;
using SchoolManagementSystem.Data.Models.JoinedModels;
using SchoolManagementSystem.Service.DTOs.Academic.Assessments;
using SchoolManagementSystem.Service.DTOs.User.Display;
using SchoolManagementSystem.Service.BusinessLogic.Factories;
using SchoolManagementSystem.Service.DTOs.Academic.Submissions;

namespace SchoolManagementSystem.Service.BusinessLogic.Utilities;

public class AcademicService
{
    private readonly RepositoryFactory _repos;
    private readonly MapperService _mapperService;
    private readonly MethodHelper _methodHelper;

    public AcademicService(RepositoryFactory repos, MapperService mapperService, MethodHelper methodHelper)
    {
        _repos = repos;
        _mapperService = mapperService;
        _methodHelper = methodHelper;
    }
    
    #region Assessments & Grades
    public async Task<BaseResponse> CreateSubmission(SubjectEnrollment subjectEnrollment, SubmissionDTO submissionDTO)
    {
        var response = new BaseResponse();

        bool submissionExists =
            await _repos.SubmissionRepository.ExistsByAssignmentAndSubjectEnrollmentIds(submissionDTO.AssignmentId,
                subjectEnrollment.Id);
        if (!submissionExists)
        {
            var toSubmissionResponse = await _mapperService.ToSubmission(submissionDTO, subjectEnrollment.Id);
            
            if (toSubmissionResponse.Success)
            {
                response = await ApplySubmission(toSubmissionResponse.Value);
            }
            else
            {
                response.SetStatus(false, toSubmissionResponse.Message);
            }
        }
        else
        {
            response.SetStatus(false, "Assignment has already been submitted by the student");
        }

        return response;
    }
    private async Task<BaseResponse> ApplySubmission(Submission submission)
    {
        var response = new BaseResponse();
        var assignmentResponse = await _repos.AssignmentRepository.GetById(submission.AssignmentId);
        if (assignmentResponse.Success)
        {
            var assignment = assignmentResponse.Value;
            submission.MarkSubmitted(assignment.IsOverdue);
            
            await _repos.SubmissionRepository.AddAsync(submission);
        }
        else
        {
            response.SetStatus(false, assignmentResponse.Message);
        }

        return response;
    }

    public async Task<BaseResponse> CreateAssessment(SubjectEnrollment subjectEnrollment, AssessmentDTO assessmentDTO)
    {
        var response = new BaseResponse();
        var submissionResponse = await _repos.SubmissionRepository.GetById(assessmentDTO.SubmissionId);

        if (submissionResponse.Success)
        {
            var submission = submissionResponse.Value;
            var toAssessmentResponse = await _mapperService.ToAssessment(assessmentDTO, submission); // reroute to assignmentId
                
            if (toAssessmentResponse.Success)
            {
                response = await ApplyAssessment(subjectEnrollment, toAssessmentResponse.Value, submission);
            }
            else
            {
                response.SetStatus(false, toAssessmentResponse.Message);
            }
        }
        else
        {
            response.SetStatus(false, submissionResponse.Message);
        }

        return response;
    }
    private async Task<BaseResponse> ApplyAssessment(SubjectEnrollment subjectEnrollment, Assessment newAssessment, Submission assessedSubmission)
    {
        return await _methodHelper.Execute<BaseResponse, BaseResponse>(async _ =>
        {
            var updateSubject = await UpdateSubjectGrade(subjectEnrollment, newAssessment.GradeValue);
            
            if (updateSubject.Success)
            {
                assessedSubmission.MarkReviewed();
                await _repos.SubmissionRepository.UpdateAsync(assessedSubmission);
                await _repos.AssessmentRepository.AddAsync(newAssessment);
            }
            
            return updateSubject;
        });
    }
    
    public async Task<BaseResponse> UpdateSubjectGrade(SubjectEnrollment subjectEnrollment, decimal newGrade)
    {
        var response = new BaseResponse();
        var assessmentsResponse = await GetAssessments(subjectEnrollment.Id);
        
        if (assessmentsResponse.Success)
        {
            var studentResponse = await _repos.UserRepository.GetById(subjectEnrollment.StudentId);

            if (studentResponse.Success)
            {
                decimal newAverage = GetNewAverageGrade(assessmentsResponse.Value, newGrade);
                subjectEnrollment.AverageGrade = newAverage;
                
                var subjectUpdatedResponse = await _repos.SubjectEnrollmentRepository
                    .UpdateAsync(subjectEnrollment);
                
                if (subjectUpdatedResponse.Success)
                {
                    // success
                    var studentUpdateResponse = await UpdateStudentGrade(studentResponse.Value, newGrade);
                    
                    if (!studentUpdateResponse.Success)
                    {
                        response.SetStatus(false, studentUpdateResponse.Message);
                    }
                }
                else
                {
                    response.SetStatus(false, subjectUpdatedResponse.Message);
                }
            }
            else
            {
                response.SetStatus(false, studentResponse.Message);
            }
        }
        else
        {
            response.SetStatus(false, assessmentsResponse.Message);
        }

        return response;
    }
    private async Task<BaseResponse> UpdateStudentGrade(User studentToUpdate, decimal newGrade)
    {
        return await _methodHelper.Execute<BaseResponse, DataResponse<List<Assessment>>>(async response =>
        {
            var assessmentsResponse = await GetAssessmentsByStudent(studentToUpdate.Id);
            
            if (assessmentsResponse.Success)
            {
                studentToUpdate.AverageGrade = GetNewAverageGrade(assessmentsResponse.Value, newGrade);
                var updated = await _repos.UserRepository.UpdateAsync(studentToUpdate);
                
                if (!updated.Success)
                {
                    response.SetStatus(false, updated.Message);
                }
            }

            return assessmentsResponse;
        });
    }
    private decimal GetNewAverageGrade(List<Assessment> assessments, decimal newGrade)
    {
        int assessmentCount = assessments.Count;
        decimal previousGradesSum = assessments.Sum(assessment => assessment.GradeValue);
        decimal newAverageGrade = (previousGradesSum + newGrade) / (assessmentCount + 1);
        
        return newAverageGrade;
    }
    public decimal GetAverageGrade(List<Assessment> assessments)
    {
        return GetAverageGrade(assessments
            .Select(a => a.GradeValue)
            .ToList());
    } 
    public decimal GetAverageGrade(List<decimal> gradeValues)
    {
        decimal averageGrade = gradeValues.Count > 0
            ? gradeValues.Average()
            : 0;
        return averageGrade;
    }
    
    public async Task<DataResponse<List<Assessment>>> GetAssessments(int studentId, int subjectId)
    {
        DataResponse<List<Assessment>> response = new();
        var subjectEnrollmentResponse = await GetSubjectEnrollment(studentId, subjectId);
        if (subjectEnrollmentResponse.Success)
        {
            var subjectEnrollmentId = subjectEnrollmentResponse.Value.Id;
            response = await GetAssessments(subjectEnrollmentId);
        }
        else
        {
            response.SetStatus(false, subjectEnrollmentResponse.Message);
        }

        return response;
    }
    private async Task<DataResponse<List<Assessment>>> GetAssessments(int subjectEnrollmentId)
    {
        var response = new DataResponse<List<Assessment>>();
        var submissionsResponse = await _repos.SubmissionRepository.GetBySubjectEnrollmentId(subjectEnrollmentId);

        if (submissionsResponse.Success)
        {
            var ids = _methodHelper.GetIds(submissionsResponse.Value);
            response = await _repos.AssessmentRepository.GetBySubmissionIds(ids);
        }
        else
        {
            response.SetStatus(false, submissionsResponse.Message);
        }

        return response;
    }
    private async Task<DataResponse<List<Assessment>>> GetAssessments(List<int> subjectEnrollmentIds)
    {
        var response = new DataResponse<List<Assessment>>();
        var submissionsResponse = await _repos.SubmissionRepository.GetBySubjectEnrollmentIds(subjectEnrollmentIds);
        
        if (submissionsResponse.Success)
        {
            var ids = _methodHelper.GetIds(submissionsResponse.Value);
            response = await _repos.AssessmentRepository.GetBySubmissionIds(ids);
        }
        else
        {
            response.SetStatus(false, submissionsResponse.Message);
        }

        return response;
    }
    private async Task<DataResponse<List<Assessment>>> GetAssessmentsByStudent(int studentId)
    {
        var response = new DataResponse<List<Assessment>>();
        var subjectEnrollmentsResponse = await _repos.SubjectEnrollmentRepository.GetByStudentId(studentId);

        if (subjectEnrollmentsResponse.Success)
        {
            var ids = await _repos.SubjectEnrollmentRepository.GetIds(subjectEnrollmentsResponse.Value);
            response = await GetAssessments(ids);
        }
        else
        {
            response.SetStatus(false, subjectEnrollmentsResponse.Message);
        }

        return response;
    }
    private async Task<DataResponse<List<Assessment>>> GetAssessmentsBySubject(int subjectId)
    {
        var response = new  DataResponse<List<Assessment>>();

        var subjectEnrollmentsResponse = await GetSubjectEnrollments(subjectId);

        if (subjectEnrollmentsResponse.Success)
        {
            var subjectEnrollmentIds = _methodHelper.GetIds(subjectEnrollmentsResponse.Value);
            response = await GetAssessments(subjectEnrollmentIds);
        }
        else
        {
            response.SetStatus(false, subjectEnrollmentsResponse.Message);
        }

        return response;
    }
    
    #endregion
    
    #region Students & Subjects
    
    public async Task<DataResponse<List<UserDisplayDTO>>> GetAllStudentsWithSubject(int subjectId)
    {
        var response = new DataResponse<List<UserDisplayDTO>>();

        var studentsResponse = await GetStudentsBySubject(subjectId);
        
        if (studentsResponse.Success)
        {
            response = await _mapperService.UsersToDisplayDTOs(studentsResponse.Value);
        }
        else
        {
            response.SetStatus(false, studentsResponse.Message);
        }
        return response;
    }
    public async Task<DataResponse<List<User>>> GetStudentsBySubject(int subjectId)
    {
        var response =  new DataResponse<List<User>>();
        var subjectEnrollments = await GetSubjectEnrollments(subjectId);
        if (subjectEnrollments.Success)
        {
            response =
                await _repos.UserRepository.GetStudentsBySubjectEnrollments(subjectEnrollments.Value);
        }
        else
        {
            response.SetStatus(false, subjectEnrollments.Message);
        }

        return response;
    }
    
    public async Task<DataResponse<SubjectEnrollment>> GetSubjectEnrollment(int studentId, int subjectId)
    {
        DataResponse<SubjectEnrollment> response = new();
        var classResponse = await _repos.SchoolClassRepository.GetClassBySubjectId(subjectId);
        if (classResponse.Success)
        {
            response = await _repos.SubjectEnrollmentRepository.GetByStudentAndClassIds(studentId, classResponse.Value.Id);
        }
        else
        {
            response.SetStatus(false, classResponse.Message);
        }

        return response;
    }
    public async Task<DataResponse<List<SubjectEnrollment>>> GetSubjectEnrollments(int subjectId)
    {
        var response = new  DataResponse<List<SubjectEnrollment>>();
        var classIdsResponse = await GetClassIdsBySubject(subjectId);
        if (classIdsResponse.Success)
        {
            response = await _repos.SubjectEnrollmentRepository.GetByClassIds(classIdsResponse.Value);
        }
        else
        {
            response.SetStatus(false, classIdsResponse.Message);
        }

        return response;
    }

    public async Task<DataResponse<List<Subject>>> GetSubjectsByStudent(int studentId)
    {
        var response = new DataResponse<List<Subject>>();

        var subjectEnrollmentsResponse = await GetSubjectEnrollments(studentId);

        if (subjectEnrollmentsResponse.Success)
        {
            var subjectEnrollmentIds = await _repos.SubjectEnrollmentRepository.GetIds(subjectEnrollmentsResponse.Value);
            var subjectsResponse = await _repos.SubjectRepository.GetBySubjectEnrollmentIds(subjectEnrollmentIds);
            
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

    private async Task<DataResponse<List<int>>> GetClassIdsBySubject(int subjectId)
    {
        return await _methodHelper.Execute<DataResponse<List<int>>, DataResponse<List<SchoolClass>>>(async response =>
        {
            var classesResponse = await _repos.SchoolClassRepository.GetClassesBySubjectId(subjectId);

            if (classesResponse.Success)
            {
                var classIds = classesResponse.Value.Select(c => c.Id).ToList();
                response.SetData(classIds);
            }

            return classesResponse;
        });
    }
    
    #endregion
    
}