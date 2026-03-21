using ProjectHelperLibrary.Response;
using SchoolManagementSystem.Data.HelperClasses;
using SchoolManagementSystem.Data.Models.Base;

namespace SchoolManagementSystem.Data.Repositories;

public class BaseRepository<T> where T : BaseModel
{
    
    private readonly SchoolContext _context;
    private readonly List<T> _collection;
    
    public BaseRepository(SchoolContext context, List<T> collection)
    {
        _context = context;
        _collection = collection;
    }
    
    #region Making changes
    
    public async Task Add(T entity)
    {
        await _collection.LoadAsync();
        _collection.Add(entity);
        await _collection.SaveAsync();
    }

    public async Task AddRange(List<T> entities)
    {
        await _collection.LoadAsync();
        _collection.AddRange(entities);
        await _collection.SaveAsync();
    }

    public async Task<BaseResponse> Update(T entity)
    {
        await _collection.LoadAsync();
        
        var index = _collection.FindIndex(e => e.Id == entity.Id);
        var response = await RepoHelper.HandleIndex(index, _collection, () => _collection[index] = entity);

        return response;
    }

    public async Task<BaseResponse> Delete(int id)
    {
        await _collection.LoadAsync();

        var index = _collection.FindIndex(e => e.Id == id);
        var response = await RepoHelper.HandleIndex(index, _collection, () => _collection.RemoveAt(index));

        return response;
    }

    #endregion
    
    #region Fetching only
    
    public async Task<DataResponse<List<T>>> GetAll()
    {
        var response = new DataResponse<List<T>>();
        await _collection.LoadAsync();
        if (!_collection.Any())
        {
            response.SetStatus(false, $"{typeof(T).Name} collection is empty");
        }
        else
        {
            response.SetData(_collection);
        }

        return response;
    }

    public async Task<DataResponse<T>> GetById(int id)
    {
        var response = new DataResponse<T>();
        
        await _collection.LoadAsync();
        
        var entity = _collection.FirstOrDefault(entity => entity.Id == id);
        if (entity is null)
        {
            response.SetStatus(false, $"Could not find {typeof(T).Name} with id: {id}");
        }
        else
        {
            response.SetData(entity);
        }

        return response;
    }
    
    #endregion
    
}