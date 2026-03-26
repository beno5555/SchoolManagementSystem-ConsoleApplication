using SchoolManagementSystem.Data.Models;
using SchoolManagementSystem.Data.Repositories.Base;

namespace SchoolManagementSystem.Data.Repositories;

public class GroupRepository : BaseRepository<Group>
{
    public GroupRepository() : base(SchoolContext.Groups)
    {
    }
}