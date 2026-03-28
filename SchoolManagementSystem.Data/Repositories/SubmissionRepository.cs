using ProjectHelperLibrary.Response;
using SchoolManagementSystem.Data.Models.Academic;
using SchoolManagementSystem.Data.Repositories.Base;

namespace SchoolManagementSystem.Data.Repositories;

public class SubmissionRepository : BaseRepository<Submission>
{
    public SubmissionRepository() : base(SchoolContext.Submissions)
    {
    }

    public async Task<DataResponse<List<Submission>>> GetBySubjectEnrollmentId(int subjectEnrollmentId)
    {
        return await GetWhere(
            submission => submission.SubjectEnrollmentId == subjectEnrollmentId,
            "Submissions associated with this subject enrollment were not found");
    }

    public async Task<DataResponse<List<Submission>>> GetBySubjectEnrollmentIds(List<int> subjectEnrollmentIds)
    {
        return await GetWhere(
            submission => subjectEnrollmentIds.Contains(submission.SubjectEnrollmentId),
            "Submissions associated with these assignments were not found");
    }
    
    public async Task<DataResponse<List<Submission>>> GetByAssignmentIds(List<int> assignmentIds)
    {
        return await GetWhere(
            submission => assignmentIds.Contains(submission.AssignmentId),
            "Submissions associated with these assignments were not found");
    }
}