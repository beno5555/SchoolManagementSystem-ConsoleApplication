using ProjectHelperLibrary.Response;
using SchoolManagementSystem.Data.Models.JoinedModels;
using SchoolManagementSystem.Data.Repositories.Base;

namespace SchoolManagementSystem.Data.Repositories;

public class SubjectEnrollmentRepository : BaseRepository<SubjectEnrollment>
{
    public SubjectEnrollmentRepository() : base(SchoolContext.SubjectEnrollments)
    {
    }

    public DataResponse<List<SubjectEnrollment>> GetSubjectEnrollmentsByClassIds(List<int> classIds)
    {
        var response = new DataResponse<List<SubjectEnrollment>>();
        var subjectEnrollments = _collection.Where(se => classIds.Contains(se.SchoolClassId)).ToList();
        if (!subjectEnrollments.Any())
        {
            response.SetStatus(false, "Could not find any subject enrollments with the given classes");
        }
        else
        {
            response.SetData(subjectEnrollments);
        }

        return response;
    }
}