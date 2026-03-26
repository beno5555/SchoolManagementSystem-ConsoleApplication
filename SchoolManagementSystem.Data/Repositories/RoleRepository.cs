using ProjectHelperLibrary.Response;
using SchoolManagementSystem.Data.Models;
using SchoolManagementSystem.Data.Repositories.Base;

namespace SchoolManagementSystem.Data.Repositories;

public class RoleRepository : BaseRepository<Role>
{
    public RoleRepository() : base(SchoolContext.Roles)
    {
        
    }
}