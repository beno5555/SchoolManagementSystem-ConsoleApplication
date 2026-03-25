using SchoolManagementSystem.Data.Config;
using SchoolManagementSystem.Data.Models;

namespace SchoolManagementSystem.Data.HelperClasses;

public static class Extensions
{
    internal static bool HasRole(this User user, SchoolEnums.RoleName roleName)
    {
        return user.Id == (int)roleName;
    }
}