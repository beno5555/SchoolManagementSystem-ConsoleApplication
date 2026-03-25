using ProjectHelperLibrary.Response;
using SchoolManagementSystem.Data.Models.JoinedModels;
using SchoolManagementSystem.Data.Repositories.Base;

namespace SchoolManagementSystem.Data.Repositories;

public class SchoolClassRepository : BaseRepository<SchoolClass>
{
    public SchoolClassRepository() : base(SchoolContext.SchoolClasses)
    {
    }

    public DataResponse<List<SchoolClass>> GetClassesBySubjectId(int subjectId)
    {
        var response = new DataResponse<List<SchoolClass>>();
        var schoolClasses = _collection.Where(entity => entity.SubjectId == subjectId).ToList();
        if (!schoolClasses.Any())
        {
            response.SetStatus(false, "Could not find any classes assigned to the subject");
        }
        else
        {
            response.SetData(schoolClasses);
        }

        return response;
    }
}