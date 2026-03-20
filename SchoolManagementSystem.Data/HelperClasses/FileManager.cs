using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;
using ProjectHelperLibrary.Response;
using ProjectHelperLibrary.Utilities;
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
    
    /// <summary>
    /// deserialize singular collection data from JSON file and set it to the list it belongs to
    /// </summary>
    public static async Task LoadAsync<T>(this List<T> collection) where T : BaseModel
    {
        var fileNameResponse = GetFileName<T>();
        await fileNameResponse.HandleResponseAsync(async res =>
        {
            string fileName = fileNameResponse.Value;
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
        });
    }
    
    /// <summary>
    /// serialize singular collection set to JSON format and save to the file it belongs to
    /// </summary>
    public static async Task SaveAsync<T>(this List<T> collection) where T : BaseModel
    {
        var fileNameResponse = GetFileName<T>();
        await fileNameResponse.HandleResponseAsync(async res =>
        {
            string fileName = fileNameResponse.Value;
            var path = AppConstants.FolderPaths.GetFullPath(fileName);

            var json = JsonSerializer.Serialize(collection, Options);
            await File.WriteAllTextAsync(path, json);
        });
    }

    
    #region Ensuring files exist
    public static async Task EnsureFileExists(Type type)
    {
        var response = GetFileName(type);
        await response.HandleResponseAsync(async res =>
        {
            string path = AppConstants.FolderPaths.GetFullPath(res.Value);
            if (!File.Exists(path))
            {
                await File.WriteAllTextAsync(path, "[]");
            }
        });
    }

    public static async Task EnsureFileExists<T>()
    {
        await EnsureFileExists(typeof(T));
    }
    
    public static async Task EnsureFoldersExist()
    {
        Directory.CreateDirectory(AppConstants.FolderPaths.DataPath);

        var types = typeof(User).Assembly.GetTypes()
            .Where(type => !type.IsAbstract 
                           && !type.IsInterface 
                           && type.IsDefined(typeof(FileNamePrefixAttribute)));

        var tasks = types.Select(EnsureFileExists); 
        await Task.WhenAll(tasks);
        // foreach (string jsonPath in AppConstants.FolderPaths.JsonPaths)
        // {
        //     await EnsureFileExists(jsonPath);
        // }
    }
    
    #endregion

    public static DataResponse<string> GetFileName<T>() where T : BaseModel
    {
        return GetFileName(typeof(T));
    }

    public static DataResponse<string> GetFileName(Type type)
    {
        var response = new DataResponse<string>();
        string fileNamePrefix = type.GetCustomAttribute<FileNamePrefixAttribute>()?.Name ?? string.Empty;
        if (!fileNamePrefix.Equals(string.Empty))
        {
            response.SetData(fileNamePrefix + ".json");
        }
        else
        {
            response.SetStatus(false, "could not find a prefix for class " + type.Name);
        }
        return response;
    }
}