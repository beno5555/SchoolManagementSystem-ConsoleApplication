using SchoolManagementSystem.Data.Models.Base;

namespace SchoolManagementSystem.Data.HelperClasses;

/// <summary>
/// helper class which automatically generates ids for new objects
/// </summary>
/// <remarks>this class can only be used in single-threaded applications like the console app. </remarks>
public static class IdGenerator
{
    public static readonly Dictionary<Type, int> MaxIds = new(30);
    public static async Task InitializeId<T>(List<T> existingCollection) where T : BaseModel
    {
        var maxId = existingCollection.Count != 0 ? existingCollection.Max(item => item.Id) : 0;
        MaxIds[typeof(T)] = maxId;
    }
    
    public static int Next(Type type)
    {
        MaxIds.TryAdd(type, 0);
        int next = ++MaxIds[type];
        return next;
    }

    // generic wrapper
    public static int Next<T>()
    {
        return Next(typeof(T));
    }
    
    public static void Track(Type type, int id)
    {
        if (MaxIds.ContainsKey(type) || MaxIds[type] < id)
        {
            MaxIds[type] = id;
        }
    }
}