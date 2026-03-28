using ProjectHelperLibrary.Response;
using SchoolManagementSystem.Data.Models.Named;
using SchoolManagementSystem.Data.Repositories.Base;

namespace SchoolManagementSystem.Data.Repositories;

public class RoleRepository : BaseRepository<Role>
{
    public RoleRepository() : base(SchoolContext.Roles)
    {
        
    }

    public async Task<DataResponse<Role>> GetByName(string name)
    {
        return await GetSingle(
            role => role.Name == name,
            $"Role '{name}' not found");
    }

    public async Task<int> GetIdByName(string name)
    {
        return await GetIdBy(role => role.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
    }
}