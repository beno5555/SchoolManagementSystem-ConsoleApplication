using SchoolManagementSystem.Data.Models;
using SchoolManagementSystem.Data.Models.Base;

namespace SchoolManagementSystem.Data.HelperClasses;

/// <summary>
/// helper class which automatically generates ids for new objects
/// </summary>
/// <remarks>this class can only be used in single-threaded applications like the console app. </remarks>
public static class IdGenerator
{
    public static readonly Dictionary<Type, int> MaxIds = new(30);
    public static void InitializeId<T>(IEnumerable<T> existingCollection) where T : BaseModel
    {
        var baseModels = existingCollection.ToList();
        var maxId = baseModels.Any() ? baseModels.Max(item => item.Id) : 0;
        MaxIds[typeof(T)] = maxId;
    }
    
    public static int Next(Type type)
    {
        MaxIds.TryAdd(type, 0);
        int next = ++MaxIds[type];
        return next;
    }
    
    

}