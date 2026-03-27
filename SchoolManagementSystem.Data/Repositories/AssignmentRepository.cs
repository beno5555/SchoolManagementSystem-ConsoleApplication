using ProjectHelperLibrary.Response;
using SchoolManagementSystem.Data.Models;
using SchoolManagementSystem.Data.Repositories.Base;

namespace SchoolManagementSystem.Data.Repositories;

public class AssignmentRepository : BaseRepository<Assignment>
{
    public AssignmentRepository() : base(SchoolContext.Assignments)
    {
        
    }

    public async Task<DataResponse<List<Assignment>>> GetByGroupClassId(int groupClassId)
    {
        return await GetWhere(
            assignment => assignment.GroupClassId == groupClassId,
            "Assignment not found"
        );
    }
}