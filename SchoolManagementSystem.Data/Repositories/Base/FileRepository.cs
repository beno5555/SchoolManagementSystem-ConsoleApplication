using ProjectHelperLibrary.Response;
using SchoolManagementSystem.Data.Config;
using SchoolManagementSystem.Data.HelperClasses;

namespace SchoolManagementSystem.Data.Repositories.Base;

public class FileRepository<T> where T : class
{
    protected readonly List<T> _collection;
    protected bool _loaded;
    
    public FileRepository(List<T> collection)
    {
        _collection = collection;
    }
    
    protected virtual async Task EnsureLoadAsync()
    {
        if (!_loaded)
        {
            await _collection.LoadAsync(AppConstants.FolderPaths.GetFullPath<T>());
            _loaded = true;
        }
    }
    
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
    
    public async Task<DataResponse<T>> GetSingle(Func<T, bool> filter, string errorMessage = "")
    {
        DataResponse<T> response = new();
        await EnsureLoadAsync();
        var entity = _collection.FirstOrDefault(filter);
        
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
    
    protected virtual async Task<DataResponse<List<T>>> GetWhere(Func<T, bool> filter, string errorMessage)
    {
        await EnsureLoadAsync();
        DataResponse<List<T>> response = new();
        var entities = _collection
            .Where(filter)
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

}