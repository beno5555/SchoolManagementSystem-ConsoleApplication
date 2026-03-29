using SchoolManagementSystem.Data.Models.Named;
using SchoolManagementSystem.Data.Repositories.Base;

namespace SchoolManagementSystem.Data.Repositories.Named;

public class GroupRepository : NamedModelRepository<Group>
{
    public GroupRepository() : base(SchoolContext.Groups)
    {
    }
}