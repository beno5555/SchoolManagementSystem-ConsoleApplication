using ProjectHelperLibrary.Response;
using SchoolManagementSystem.Data.Models.JoinedModels;
using SchoolManagementSystem.Data.Repositories.Base;

namespace SchoolManagementSystem.Data.Repositories.Joined;

public class SchoolClassRepository : BaseRepository<SchoolClass>
{
    public SchoolClassRepository() : base(SchoolContext.SchoolClasses)
    {
    }

    public async Task<DataResponse<List<SchoolClass>>> GetClassesBySubjectId(int subjectId)
    {
        var response = await GetWhere(
            schoolClass => schoolClass.SubjectId == subjectId,
            "Could not find any classes assigned to the subject");
        return response;
    }
    
    public async Task<DataResponse<List<SchoolClass>>> GetByTeacherId(int teacherId)
    {
        var response = await GetWhere(
            schoolClass => schoolClass.TeacherId == teacherId,
            "Could not find any classes assigned to the teacher");
        return response;
    }

    public async Task<DataResponse<SchoolClass>> GetBySubjectAndTeacherId(int subjectId, int teacherId)
    {
        var response = await GetSingle(
            schoolClass =>
                schoolClass.SubjectId == subjectId &&
                schoolClass.TeacherId == teacherId,
            "Could not find a class assigned to the teacher and the subject");
        return response;
    }
 
    public async Task<DataResponse<SchoolClass>> GetClassBySubjectId(int subjectId)
    {
        var response = await GetSingle(
            schoolClass => schoolClass.SubjectId == subjectId,
            "Class assigned to the teacher not found");
        return response;
    }

    public async Task<bool> ExistsByTeacherAndSubjectIds(int subjectId, int teacherId)
    {
        return await ExistsAsync(
            schoolClass => schoolClass.TeacherId == teacherId && 
                           schoolClass.SubjectId == subjectId);
    }
}