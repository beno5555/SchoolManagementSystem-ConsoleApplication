using SchoolManagementSystem.Service;
using SchoolManagementSystem.Service.Services;

namespace SchoolManagementSystem;

public static class Menu
{
    private static UserService _userService = new();
    public static async Task Run()
    {
        await Initializer.Execute();
    }
}