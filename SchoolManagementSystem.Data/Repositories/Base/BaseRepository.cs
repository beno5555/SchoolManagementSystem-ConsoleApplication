using ProjectHelperLibrary.Response;
using SchoolManagementSystem.Data.Config;
using SchoolManagementSystem.Data.HelperClasses;
using SchoolManagementSystem.Data.Models.Base;

namespace SchoolManagementSystem.Data.Repositories.Base;

public class BaseRepository<T> : FileRepository<T> where T : BaseModel
{
    
    protected BaseRepository(List<T> collection) : base(collection)
    {
    }
    
    #region Load & Save Helper 

    protected override async Task EnsureLoadAsync()
    {
        if (!_loaded)
        {
            await _collection.LoadAsync();
            _loaded = true;
        }
    }
    
    #endregion
    
    #region Modifying
    
    private void AddInternal(T entity)
    {
        entity.Id = IdGenerator.Next<T>();
        _collection.Add(entity);
    }
    
    public async Task AddAsync(T entity)
    {
        await EnsureLoadAsync();
        AddInternal(entity);
        await _collection.SaveAsync();
    }

    public async Task AddRangeAsync(IEnumerable<T> entities)
    {
        await EnsureLoadAsync();
        foreach (var entity in entities)
        {
            AddInternal(entity);
        }
        await _collection.SaveAsync();
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
            await _collection.SaveAsync();
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
            await _collection.SaveAsync();
        }
        else
        {
            response.SetStatus(false, $"{typeof(T).Name} entity not found");
        }
        
        return response;
    }

    #endregion
    
    #region Reading only

    public async Task<DataResponse<T>> GetById(int id)
    {
        var response = await GetSingle(
            filter: entity => entity.Id == id, 
            $"Could not find {typeof(T).Name} with id: {id}"); 
        return response;
    }

    public async Task<int> GetIdBy(Func<T, bool> filter)
    {
        var entityResponse = await GetSingle(filter);
        var result = entityResponse.Success ? entityResponse.Value.Id : -1;
        return result;
    }

    public async Task<List<int>> GetIds(List<T> entities)
    {
        return entities.Select(entity => entity.Id).Distinct().ToList();
    }

    public async Task<bool> ExistsAsync(int id)
    {
        await EnsureLoadAsync();
        bool exists = _collection.Any(entity => entity.Id == id);
        return exists;
    }

    public async Task<bool> ExistsAsync(Func<T, bool> filter)
    {
        await EnsureLoadAsync();
        bool exists = _collection.Any(filter);
        return exists;
    }

    protected override async Task<DataResponse<List<T>>> GetWhere(Func<T, bool> filter, string errorMessage)
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