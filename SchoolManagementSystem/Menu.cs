using ProjectHelperLibrary.Utilities;
using SchoolManagementSystem.Service;
using SchoolManagementSystem.Service.BusinessLogic;
using SchoolManagementSystem.Service.BusinessLogic.Auth;
using SchoolManagementSystem.Service.Display;
using SchoolManagementSystem.Service.DTOs.User.Auth;

namespace SchoolManagementSystem;

public static class Menu
{
    private static readonly UserService UserService = new();
    private static readonly AuthService AuthService = new();
    public static async Task Run()
    {
        await Initializer.Execute();

        var user = new AdminRegisterDTO
        {
            FirstName = "meore sandro",
            LastName = "benashvili",
            Address = "yvelgan",
            Email = "abc@gmail.com",
            DateOfBirth = new DateTime(1980, 1, 1),
            Password = "123456",
            PhoneNumber = "4343",
            PrivateId = "45434343",
            RoleId = 4,
            
        };

        var registerResponse = await AuthService.RegisterUser(user);
        if (registerResponse.Success)
        {
            Console.WriteLine("User successfully registered");
        }
        else
        {
            Console.WriteLine(registerResponse.Message);
        }

        ConsoleUtilities.ResetMenu();

        var getUserResponse = await UserService.GetUserById(3);
        if (getUserResponse.Success)
        {
            Console.WriteLine("User successfully retrieved");
            DisplayManager.Print(getUserResponse.Value, " - ");
        }
        
        ConsoleUtilities.ResetMenu(userMessage: "Press any key to exit menu...");
    }
}