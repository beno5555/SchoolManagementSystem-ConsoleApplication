using System.Runtime.CompilerServices;
using ProjectHelperLibrary.Response;
using SchoolManagementSystem.Data.Models.JoinedModels;
using SchoolManagementSystem.Data.Repositories.Base;

namespace SchoolManagementSystem.Data.Repositories.Joined;

public class GroupClassRepository : BaseRepository<GroupClass>
{
    public GroupClassRepository() : base(SchoolContext.GroupClasses)
    {
        
    }

    public async Task<DataResponse<GroupClass>> GetByGroupAndClassIdAsync(int groupId, int classId)
    {
        return await GetSingle(
            groupClass => groupClass.GroupId == groupId &&
                          groupClass.SchoolClassId == classId,
            "Could not find group class");
    }

    public async Task<DataResponse<List<GroupClass>>> GetByClassIdAsync(int classId)
    {
        return await GetWhere(
            groupClass => groupClass.SchoolClassId == classId,
            "Could not find groupclass for the group");
    }
}