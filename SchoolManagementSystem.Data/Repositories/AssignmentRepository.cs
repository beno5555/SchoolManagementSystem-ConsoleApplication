using SchoolManagementSystem.Data.Models;
using SchoolManagementSystem.Data.Repositories.Base;

namespace SchoolManagementSystem.Data.Repositories;

public class AssignmentRepository : BaseRepository<Assignment>
{
    public AssignmentRepository(List<Assignment> collection) : base(collection)
    {
        
    }
}