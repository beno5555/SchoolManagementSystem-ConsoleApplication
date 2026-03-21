using SchoolManagementSystem.Data.Models;

namespace SchoolManagementSystem.Data.Repositories;

public class AssignmentRepository : BaseRepository<Assignment>
{
    public AssignmentRepository(SchoolContext context, List<Assignment> collection) : base(context, collection)
    {
        
    }
}