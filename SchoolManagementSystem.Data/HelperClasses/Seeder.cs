using SchoolManagementSystem.Data.Config;
using SchoolManagementSystem.Data.Models;
using SchoolManagementSystem.Data.Models.Base;
using SchoolManagementSystem.Data.Models.Named;

namespace SchoolManagementSystem.Data.HelperClasses;

public static class Seeder
{
    /// <summary>
    /// adds T type objects to List of type T collection with predefined enum names (given that collection has 0 items)
    /// </summary>
    public static async Task SeedEnums<TEnum, TModel>(List<TModel> collection, bool loaded = false) 
        where TModel : NamedModel, new()
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
                TModel newObject = new TModel
                {
                    Id = IdGenerator.Next<TModel>(),
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
                    "ncE/vkagQZft0U5DxV0Z4IbHNBWgVkt/1RC/haf3nPg=",
                    "oNsJmAzkVehBjvRvQta4DtP3DveFpzniZ50nST4F2Pg=",
                    (int)adminRoleId);
                admin.Id = IdGenerator.Next<User>();
                users.Add(admin);
            }
            await users.SaveAsync();
        }

    }
}