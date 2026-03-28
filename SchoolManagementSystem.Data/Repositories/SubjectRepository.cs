using ProjectHelperLibrary.Response;
using SchoolManagementSystem.Data.Models.Named;
using SchoolManagementSystem.Data.Repositories.Base;

namespace SchoolManagementSystem.Data.Repositories;

public class SubjectRepository : BaseRepository<Subject>
{
    public SubjectRepository() : base(SchoolContext.Subjects) 
    {
        
    }

    public async Task<DataResponse<List<Subject>>> GetBySubjectEnrollmentIds(List<int> subjectEnrollmentIds)
    {
        return await GetWhere(
            subject => subjectEnrollmentIds.Contains(subject.Id),
            "No subjects associated with the following enrollments");
    }
}