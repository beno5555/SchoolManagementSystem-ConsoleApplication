using System.Collections;
using System.Text.Json;
using System.Text.Json.Serialization;
using ProjectHelperLibrary.Response;
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
    public static async Task LoadAsync<T>(string fileName, List<T> collection) where T : BaseModel
    {
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
            await ForceFile(fileName);
        }
        
        collection.Clear();
        collection.AddRange(deserializedCollection);
    }
    
    /// <summary>
    /// serialize singular collection set to JSON format and save to the file it belongs to
    /// </summary>
    public static async Task SaveAsync<T>(string fileName, List<T> collection) where T : BaseModel
    {
        var path = AppConstants.FolderPaths.GetFullPath(fileName);
        
        var json = JsonSerializer.Serialize(collection, Options);
        await File.WriteAllTextAsync(path, json);
    }

    public static async Task ForceFile(string fileName)
    {
        var path = AppConstants.FolderPaths.GetFullPath(fileName);
        if (!File.Exists(path))
        {
            await File.WriteAllTextAsync(path, "[]");
        }
    }
    
    public static async Task ForceStorage()
    {
        Directory.CreateDirectory(AppConstants.FolderPaths.DataPath);

        foreach (string jsonPath in AppConstants.FolderPaths.JsonPaths)
        {
            await FileManager.ForceFile(jsonPath);
        }
    }

    public static string GetFileName(string collectionName)
    {
        return collectionName.ToLower() + ".json";
    }
}