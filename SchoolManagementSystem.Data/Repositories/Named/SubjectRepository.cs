using ProjectHelperLibrary.Response;
using SchoolManagementSystem.Data.Models.Named;
using SchoolManagementSystem.Data.Repositories.Base;

namespace SchoolManagementSystem.Data.Repositories.Named;

public class SubjectRepository : NamedModelRepository<Subject>
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

    public async Task<DataResponse<List<Subject>>> GetByNotIds(List<int> subjectIds)
    {
        return await GetWhere(
            subject => !subjectIds.Contains(subject.Id),
            "Could not retrieve any subjects excluding the ones told to exclude");
    }
}