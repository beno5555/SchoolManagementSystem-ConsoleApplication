using System.Collections;
using System.Text.Json;
using ProjectHelperLibrary.Response;
using SchoolManagementSystem.Data.Models;

namespace SchoolManagementSystem.Data.HelperClasses;

public static class FileManager
{
    /// <summary>
    /// deserialize singular collection data from JSON file and set it to the list it belongs to
    /// </summary>
    public static async Task<List<T>> LoadAsync<T>(string fileName) where T : BaseModel
    {
        var path = FolderPaths.GetFullPath(fileName);
        var result = new List<T>();
        if (File.Exists(path))
        {
            var json = await File.ReadAllTextAsync(path);
            result = JsonSerializer.Deserialize<List<T>>(json) ?? [];
        }
        return result;
    }


    // public static async Task<object> LoadAsync(Type itemType, string fileName)
    // {
    //     var path = FolderPaths.GetFullPath(fileName);
    //
    //     Type listType = typeof(List<>).MakeGenericType(itemType);
    //
    //     if (File.Exists(path))
    //     {
    //         var json = await File.ReadAllTextAsync(path);
    //     
    //         var result = JsonSerializer.Deserialize(json, listType);
    //     
    //         return result ?? Activator.CreateInstance(listType)!;
    //     }
    //
    //     // Return a new empty list of the correct type if file doesn't exist
    //     return Activator.CreateInstance(listType)!;
    // }
    
    
    
    /// <summary>
    /// serialize singular collection set to JSON format and save to the file it belongs to
    /// </summary>
    public static async Task SaveAsync<T>(string fileName, List<T> collection) where T : BaseModel
    {
        var path = FolderPaths.GetFullPath(fileName);
        if (File.Exists(path))
        {
            var options = new JsonSerializerOptions
            {
                WriteIndented = true
            };
            var json = JsonSerializer.Serialize(collection, options);
            await File.WriteAllTextAsync(path, json);
        }
    }
    
    // public static async Task SaveAsync(object collection, string fileName, Type itemType)
    // {
    //     var path = FolderPaths.GetFullPath(fileName);
    //
    //     var options = new JsonSerializerOptions
    //     {
    //         WriteIndented = true
    //     };
    //
    //     Type listType = typeof(List<>).MakeGenericType(itemType);
    //
    //     var json = JsonSerializer.Serialize(collection, listType, options);
    //
    //     await File.WriteAllTextAsync(path, json);
    // }

    public static async Task ForceFile(string fileName)
    {
        var path = FolderPaths.GetFullPath(fileName);
        if (!File.Exists(path))
        {
            await File.WriteAllTextAsync(path, "[]");
        }
    }
}