using SchoolManagementSystem.Data.Models.JoinedModels;
using SchoolManagementSystem.Data.Repositories.Base;

namespace SchoolManagementSystem.Data.Repositories.Joined;

public class GroupClassRepository : BaseRepository<GroupClass>
{
    public GroupClassRepository() : base(SchoolContext.GroupClasses)
    {
        
    }
}