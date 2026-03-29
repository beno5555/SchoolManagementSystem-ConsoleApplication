using ProjectHelperLibrary.Response;
using SchoolManagementSystem.Data.Models.Academic;
using SchoolManagementSystem.Data.Repositories.Base;

namespace SchoolManagementSystem.Data.Repositories.Academic;

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

    public async Task<DataResponse<Submission>> GetByAssignmentAndSubjectEnrollmentIds(int assignmentId,
        int subjectEnrollmentId)
    {
        return await GetSingle(
            submission => submission.AssignmentId == assignmentId &&
                          submission.SubjectEnrollmentId == subjectEnrollmentId,
            "Submission linked to the assignment and enrollment were not found");
    }

    public async Task<bool> ExistsByAssignmentAndSubjectEnrollmentIds(int assignmentId,
        int subjectEnrollmentId)
    {
        return await ExistsAsync(submission => submission.AssignmentId == assignmentId &&
                                               submission.SubjectEnrollmentId == subjectEnrollmentId);
    }
}