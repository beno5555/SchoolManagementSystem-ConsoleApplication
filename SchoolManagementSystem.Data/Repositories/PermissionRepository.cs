using SchoolManagementSystem.Data.Models;
using SchoolManagementSystem.Data.Repositories.Base;

namespace SchoolManagementSystem.Data.Repositories;

public class PermissionRepository : BaseRepository<Permission>
{
    public PermissionRepository(List<Permission> collection) : base(collection)
    {
        
    }
}