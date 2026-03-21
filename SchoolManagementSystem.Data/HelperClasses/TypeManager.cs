using System.Reflection;
using SchoolManagementSystem.Data.Models;

namespace SchoolManagementSystem.Data.HelperClasses;

public static class TypeManager
{
    public static Type[] GetClassTypesWithAttributes<T>()
    {
        var types = typeof(User).Assembly.GetTypes()
            .Where(type => !type.IsAbstract 
                           && !type.IsInterface 
                           && type.IsDefined(typeof(T)));
        return types.ToArray();
    }
}