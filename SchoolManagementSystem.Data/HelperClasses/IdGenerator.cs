using SchoolManagementSystem.Data.Models;

namespace SchoolManagementSystem.Data.HelperClasses;

/// <summary>
/// helper class which automatically generates ids for new objects
/// </summary>
/// <remarks>this class can only be used in single-threaded applications like the console app. </remarks>
public static class IdGenerator
{
    private static readonly Dictionary<Type, int> MaxIds = new(30);
    public static void InitializeId<T>(List<T> existingCollection) where T : BaseModel
    {
        var maxId = existingCollection.Any() ? existingCollection.Max(item => item.Id) : 0;
        MaxIds[typeof(T)] = maxId;
    }
    
    public static int Next(Type type)
    {
        MaxIds.TryAdd(type, 0);
        int next = ++MaxIds[type];
        return next;
    }

}