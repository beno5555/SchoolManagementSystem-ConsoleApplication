using SchoolManagementSystem.Data.Config;
using SchoolManagementSystem.Data.Models;
using SchoolManagementSystem.Data.Models.Base;
using SchoolManagementSystem.Data.Models.JoinedModels;
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
    
    public static async Task SeedRolePermissions(List<RolePermission> rolePermissions, List<Role> roles, List<Permission> permissions)
    {
        if (rolePermissions.Count != 0) return;

        var rolePermissionMappings = new Dictionary<SchoolEnums.RoleName, List<SchoolEnums.PermissionName>>
        {
            [SchoolEnums.RoleName.Student] = new()
            {
                SchoolEnums.PermissionName.ViewOwnGrade,
                SchoolEnums.PermissionName.ViewHomeworks,
                SchoolEnums.PermissionName.SubmitHomework,
                SchoolEnums.PermissionName.RateTeacher
            },
            [SchoolEnums.RoleName.Teacher] = new()
            {
                SchoolEnums.PermissionName.ViewStudentGrade,
                SchoolEnums.PermissionName.GetOwnStudent,
                SchoolEnums.PermissionName.GetSubjects,
                SchoolEnums.PermissionName.UploadHomework,
                SchoolEnums.PermissionName.AssessStudent,
                SchoolEnums.PermissionName.ScheduleTest
            },
            [SchoolEnums.RoleName.Principal] = new()
            {
                SchoolEnums.PermissionName.ViewAnyGrade,
                SchoolEnums.PermissionName.GetAllMembers,
                SchoolEnums.PermissionName.GetAverageGradeOfAnyKind,
                SchoolEnums.PermissionName.AddMember,
                SchoolEnums.PermissionName.RemoveMember,
                SchoolEnums.PermissionName.GetMember,
                SchoolEnums.PermissionName.GetSubjects,
                SchoolEnums.PermissionName.AssessStudent
            },
            [SchoolEnums.RoleName.SuperAdmin] = Enum.GetValues<SchoolEnums.PermissionName>().ToList()
        };

        foreach (var (roleName, permissionNames) in rolePermissionMappings)
        {
            var role = roles.FirstOrDefault(r => r.Name == roleName.ToString());
            if (role is null) continue;

            foreach (var permissionName in permissionNames)
            {
                var permission = permissions.FirstOrDefault(p => p.Name == permissionName.ToString());
                if (permission is null) continue;

                rolePermissions.Add(new RolePermission(role.Id, permission.Id));
            }
        }

        await rolePermissions.SaveAsync();
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