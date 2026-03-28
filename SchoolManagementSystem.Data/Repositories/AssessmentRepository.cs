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

    
}