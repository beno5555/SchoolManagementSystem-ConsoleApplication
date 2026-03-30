using ProjectHelperLibrary.Response;
using SchoolManagementSystem.Data.Models.Base;

namespace SchoolManagementSystem.Data.Repositories.Base;

public class NamedModelRepository<T> : BaseRepository<T> where T : NamedModel
{
    protected NamedModelRepository(List<T> collection) : base(collection)
    {
        
    }
    
    public async Task<DataResponse<T>> GetByName(string name)
    {
        return await GetSingle(
            role => role.Name == name,
            $"{nameof(T)} '{name}' not found");
    }

    public async Task<int> GetIdByName(string name)
    {
        return await GetIdBy(role => role.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
    }

    public async Task<DataResponse<List<string>>> GetNames()
    {
        var response = new DataResponse<List<string>>();
        var all = await GetAll();
        if (all.Success)
        {
            List<string> names = all.Value.Select(entity => entity.Name).ToList();
            response.SetData(names);
        }
        else
        {
            response.SetStatus(false, all.Message);
        }

        return response;
    }
}