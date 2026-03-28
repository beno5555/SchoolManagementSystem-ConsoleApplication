using SchoolManagementSystem.Data.Models.Named;
using SchoolManagementSystem.Data.Repositories.Base;

namespace SchoolManagementSystem.Data.Repositories;

public class PermissionRepository : BaseRepository<Permission>
{
    public PermissionRepository() : base(SchoolContext.Permissions)
    {
        
    }
}