using ProjectHelperLibrary.Response;
using SchoolManagementSystem.Data.Models.Academic;
using SchoolManagementSystem.Data.Repositories.Base;

namespace SchoolManagementSystem.Data.Repositories.Academic;

public class AssessmentRepository : BaseRepository<Assessment>
{
    public AssessmentRepository() : base(SchoolContext.Assessments)
    {
        
    }

    // public async Task<DataResponse<List<Assessment>>> GetBySubjectEnrollmentId(int subjectEnrollmentId)
    // {
    //     var response = await GetWhere(
    //         assessment => assessment.SubjectEnrollmentId == subjectEnrollmentId,
    //         "Assessments for this subject enrollment not found");
    //     return response;
    // }

    public async Task<DataResponse<List<Assessment>>> GetBySubmissionIds(List<int> submissionIds)
    {
        return await GetWhere(
            assessment => submissionIds.Contains(assessment.SubmissionId),
            "Assessments for this submission not found");
    }


    // public async Task<DataResponse<List<Assessment>>> GetBySubjectEnrollmentIds(List<int> ids)
    // {
    //     return await GetWhere(
    //         assessment => ids.Contains(assessment.SubjectEnrollmentId),
    //         "Assessments for this subject enrollment could not be found");
    // }
}