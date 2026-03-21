using SchoolManagementSystem.Data.Models;

namespace SchoolManagementSystem.Data.Repositories;

public class PermissionRepository : BaseRepository<Permission>
{
    public PermissionRepository(SchoolContext context, List<Permission> collection) : base(context, collection)
    {
        
    }
}