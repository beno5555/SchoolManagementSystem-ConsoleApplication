using System.Text.Json;
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

    /// <summary>
    /// serialize singular collection set to JSON format and save to the file it belongs to
    /// </summary>
    public static async Task SaveAsync<T>(string fileName, List<T> collection) where T : BaseModel
    {
        var path = Path.Combine(FolderPaths.DataPath, fileName);
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

    public static async Task ForceFile(string fileName)
    {
        var path = FolderPaths.GetFullPath(fileName);
        if (!File.Exists(path))
        {
            await File.WriteAllTextAsync(path, "[]");
        }
    }
}