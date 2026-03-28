using ProjectHelperLibrary.Response;
using SchoolManagementSystem.Data.Models;
using SchoolManagementSystem.Data.Repositories.Base;

namespace SchoolManagementSystem.Data.Repositories;

public class AssessmentRepository : BaseRepository<Assessment>
{
    public AssessmentRepository() : base(SchoolContext.Assessments)
    {
        
    }

    public async Task<DataResponse<List<Assessment>>> GetBySubjectEnrollmentId(int subjectEnrollmentId)
    {
        var response = await GetWhere(
            assessment => assessment.SubjectEnrollmentId == subjectEnrollmentId,
            "Assessments for this subject enrollment not found");
        return response;
    }


    public async Task<DataResponse<List<Assessment>>> GetBySubjectEnrollmentIds(List<int> ids)
    {
        return await GetWhere(
            assessment => ids.Contains(assessment.SubjectEnrollmentId),
            "Assessments for this subject enrollment could not be found");
    }
}