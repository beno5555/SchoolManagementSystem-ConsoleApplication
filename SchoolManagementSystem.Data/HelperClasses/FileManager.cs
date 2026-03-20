using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;
using SchoolManagementSystem.Data.Config;
using SchoolManagementSystem.Data.Models;
using SchoolManagementSystem.Data.Models.Base;

namespace SchoolManagementSystem.Data.HelperClasses;

public static class FileManager
{

    #region Properties

    private static readonly JsonSerializerOptions Options = new()
    {
        WriteIndented = true,
        DefaultIgnoreCondition = JsonIgnoreCondition.Never
    };

    #endregion
    
    #region Load & Save
    
    /// <summary>
    /// deserialize singular collection data from JSON file and set it to the list it belongs to
    /// </summary>
    public static async Task LoadAsync<T>(this List<T> collection) 
    {
        string fileName = AppConstants.FolderPaths.GetFileName<T>();
        var path = AppConstants.FolderPaths.GetFullPath(fileName);
        var deserializedCollection = new List<T>();
        
        if (File.Exists(path))
        {
            var json = await File.ReadAllTextAsync(path);
            bool isValid = !string.IsNullOrWhiteSpace(json);
            deserializedCollection = isValid
                ? JsonSerializer.Deserialize<List<T>>(json, Options) ?? []
                : [];
        }
        else
        {
            await EnsureFileExists<T>();
        }
        
        collection.Clear();
        collection.AddRange(deserializedCollection);
    }
    
    /// <summary>
    /// serialize singular collection set to JSON format and save to the file it belongs to
    /// </summary>
    public static async Task SaveAsync<T>(this List<T> collection) where T : BaseModel
    {
        var fileName = AppConstants.FolderPaths.GetFileName<T>();
        var path = AppConstants.FolderPaths.GetFullPath(fileName);
        
        var json = JsonSerializer.Serialize(collection, Options);
        await File.WriteAllTextAsync(path, json);
    }

    #endregion
    
    #region Ensuring files exist
    public static async Task EnsureFileExists(Type type)
    {
        var fileName = AppConstants.FolderPaths.GetFileName(type);
        string path = AppConstants.FolderPaths.GetFullPath(fileName);
        if (!File.Exists(path))
        {
            await File.WriteAllTextAsync(path, "[]");
        }
        
        if (!AppConstants.FolderPaths.JsonPaths.Contains(path))
        {
            AppConstants.FolderPaths.JsonPaths.Add(path);   
        }
    }

    public static async Task EnsureFileExists<T>()
    {
        await EnsureFileExists(typeof(T));
    }
    
    public static async Task EnsureFoldersExist()
    {
        Directory.CreateDirectory(AppConstants.FolderPaths.DataPath);

        var types = GetClassTypesWithAttributes<FileNamePrefixAttribute>();
        var tasks = types.Select(EnsureFileExists); 
        await Task.WhenAll(tasks);
    }

    public static Type[] GetClassTypesWithAttributes<T>()
    {
        var types = typeof(User).Assembly.GetTypes()
            .Where(type => !type.IsAbstract 
                           && !type.IsInterface 
                           && type.IsDefined(typeof(T)));
        return types.ToArray();
    }
    
    #endregion
    
}