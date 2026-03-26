using ProjectHelperLibrary.Response;
using SchoolManagementSystem.Data.HelperClasses;
using SchoolManagementSystem.Data.Models.Base;

namespace SchoolManagementSystem.Data.Repositories.Base;

public class BaseRepository<T> where T : BaseModel
{
    protected readonly List<T> _collection;
    private bool _loaded;
    private bool _isDirty;
    
    protected BaseRepository(List<T> collection)
    {
        _collection = collection;
    }
    
    #region Load & Save Helper 

    protected async Task EnsureLoadAsync()
    {
        if (!_loaded)
        {
            await _collection.LoadAsync();
            _loaded = true;
        }
    }
    
    protected async Task SaveAsync()
    {
        if (_isDirty)
        {
            await _collection.SaveAsync();
            _isDirty = false;
        }
    }
    
    #endregion
    
    #region Modifying
    
    private void AddInternal(T entity)
    {
        entity.Id = IdGenerator.Next<T>();
        _collection.Add(entity);
        _isDirty = true;
    }
    
    public async Task AddAsync(T entity)
    {
        await EnsureLoadAsync();
        AddInternal(entity);
    }

    public async Task AddRangeAsync(IEnumerable<T> entities)
    {
        await EnsureLoadAsync();
        foreach (var entity in entities)
        {
            AddInternal(entity);
        }
    }

    public async Task<BaseResponse> UpdateAsync(T entity)
    {
        var response = new BaseResponse();
        await EnsureLoadAsync();
        
        var entityFound = _collection.FirstOrDefault(e => e.Id == entity.Id);
        if (entityFound is not null)
        {
            int index = _collection.IndexOf(entityFound);
            _collection[index] = entity;
            _isDirty = true;
        }
        else
        {
            response.SetStatus(false, $"{typeof(T).Name} entity not found");
        }
        
        return response;
    }

    public async Task<BaseResponse> DeleteAsync(int id)
    {
        var response = new BaseResponse();
        await EnsureLoadAsync();

        var entity = _collection.FirstOrDefault(e => e.Id == id);
        if (entity is not null)
        {
            _collection.Remove(entity);
            _isDirty = true;
        }
        else
        {
            response.SetStatus(false, $"{typeof(T).Name} entity not found");
        }
        
        return response;
    }

    #endregion
    
    #region Reading only
    
    public async Task<DataResponse<List<T>>> GetAll()
    {
        var response = new DataResponse<List<T>>();
        await EnsureLoadAsync();
        
        if (_collection.Count != 0)
        {
            response.SetData(_collection.ToList()); // passing a copy
        }
        else
        {
            response.SetStatus(false, $"{typeof(T).Name} collection is empty");
        }

        return response;
    }

    public async Task<DataResponse<T>> GetById(int id)
    {
        var response = await GetSingle(
            filter: entity => entity.Id == id, 
            $"Could not find {typeof(T).Name} with id: {id}"); 
        return response;
    }

    public async Task<bool> ExistsAsync(int id)
    {
        await EnsureLoadAsync();
        bool exists = _collection.Any(entity => entity.Id == id);
        return exists;
    }
    
    public async Task<DataResponse<T>> GetSingle(Func<T, bool> filter, string errorMessage)
    {
        await EnsureLoadAsync();
        DataResponse<T> response = new();
        var entity = _collection
            .FirstOrDefault(filter)
            ;
        if (entity is not null)
        {
            response.SetData(entity);
        }
        else
        {
            response.SetStatus(false, errorMessage);
        }

        return response;
    }

    protected async Task<DataResponse<List<T>>> GetWhere(Func<T, bool> filter, string errorMessage)
    {
        await EnsureLoadAsync();
        DataResponse<List<T>> response = new();
        var entities = _collection
            .Where(filter)
            .DistinctBy(entity => entity.Id)
            .ToList();
        if (entities.Count != 0)
        {
            response.SetData(entities);
        }
        else
        {
            response.SetStatus(false, errorMessage);
        }

        return response;
    }
    
    #endregion
    
}