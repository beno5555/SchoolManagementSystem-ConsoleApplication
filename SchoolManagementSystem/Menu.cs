using ProjectHelperLibrary.Utilities;
using SchoolManagementSystem.Service;
using SchoolManagementSystem.Service.BusinessLogic;
using SchoolManagementSystem.Service.DTOs.User.Auth;

namespace SchoolManagementSystem;

public static class Menu
{
    private static readonly UserService _userService = new();
    private static readonly AuthService _authService = new();
    public static async Task Run()
    {
        await Initializer.Execute();

        var user = new RegisterDTO
        {
            FirstName = "sandro",
            LastName = "benashvili",
            Address = "yvelgan",
            Email = "abc@gmail.com",
            DateOfBirth = new DateTime(1980, 1, 1),
            Password = "123456",
            PhoneNumber = "4343",
            PrivateId = "45434343",
            RoleId = 4
        };

        var registerResponse = await _authService.RegisterUser(user);
        if (registerResponse.Success)
        {
            Console.WriteLine("User successfully registered");
        }
        else
        {
            Console.WriteLine(registerResponse.Message);
        }

        ConsoleUtilities.ResetMenu();




    }
}