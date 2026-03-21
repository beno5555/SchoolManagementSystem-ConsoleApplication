using SchoolManagementSystem.Data.Models;

namespace SchoolManagementSystem.Data.Repositories;

public class SubjectRepository : BaseRepository<Subject>
{
    public SubjectRepository(SchoolContext context, List<Subject> collection) : base(context, collection)
    {
        
    }
}