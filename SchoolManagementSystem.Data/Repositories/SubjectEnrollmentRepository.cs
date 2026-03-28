using ProjectHelperLibrary.Response;
using SchoolManagementSystem.Data.Models.JoinedModels;
using SchoolManagementSystem.Data.Repositories.Base;

namespace SchoolManagementSystem.Data.Repositories;

public class SubjectEnrollmentRepository : BaseRepository<SubjectEnrollment>
{
    public SubjectEnrollmentRepository() : base(SchoolContext.SubjectEnrollments)
    {
    }

    public async Task<DataResponse<List<SubjectEnrollment>>> GetByClassIds(List<int> classIds)
    {
        var response = await GetWhere(
            se => classIds.Contains(se.SchoolClassId),
            "Subject enrollment in the given classes were not found");
        return response;
    }

    public async Task<DataResponse<SubjectEnrollment>> GetByStudentAndClassIds(int studentId, int classId)
    {
        var response = await GetSingle(
            se =>
                se.StudentId == studentId &&
                se.SchoolClassId == classId,
            "Subject Enrollment for the current student not found");
        return response;
    }

}