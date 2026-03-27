using SchoolManagementSystem.Data.Models;
using System.Reflection;

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