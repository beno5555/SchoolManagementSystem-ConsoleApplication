using SchoolManagementSystem.Data.Config;
using SchoolManagementSystem.Data.Models;
using SchoolManagementSystem.Data.Models.Base;

namespace SchoolManagementSystem.Data.HelperClasses;

public static class Seeder
{
    /// <summary>
    /// adds T type objects to List of type T collection with predefined enum names given that collection has 0 items
    /// </summary>
    public static async Task SeedEnums<TEnum, T>(List<T> collection, bool loaded = false) 
        where T : NamedModel, new()
        where TEnum : struct, Enum
    {
        if (!loaded)
        {
            await collection.LoadAsync();
        }
        if (!collection.Any())
        {
            var results = Enum.GetNames<TEnum>().Select(enumName =>
            {
                T newObject = new T
                {
                    Id = IdGenerator.Next<T>(),
                    Name = enumName,
                };
                return newObject;
            });
            collection.AddRange(results);
            await collection.SaveAsync();
        }
    }
    /// <summary>
    /// adds a superadmin to the users list if there already is not one. (superadmin data is hardcoded)
    /// </summary>
    public static async Task SeedSuperAdmin(List<User> users, List<Role> roles, bool loaded = false)
    {
        if (!loaded)
        {
            await users.LoadAsync();
        }
        if(users.Count == 0)
        {
            int? adminRoleId = roles.FirstOrDefault(role => role.Name == nameof(SchoolEnums.RoleName.SuperAdmin))?.Id;
            bool adminExists = users
                .Any(user => user.RoleId == adminRoleId);
            if (!adminExists && adminRoleId.HasValue)
            {
                User admin = new(
                    "superadmin",
                    "superadmin",
                    "5914343433",
                    "Tbilisi, GE",
                    new DateTime(2009, 04, 17),
                    "01255339127",
                    "superadmin@gmail.com",
                    "123",
                    (int)adminRoleId);
                admin.Id = IdGenerator.Next<User>();
                users.Add(admin);
            }

            
            await users.SaveAsync();
        }

    }
}