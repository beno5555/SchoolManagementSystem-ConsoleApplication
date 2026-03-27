using ProjectHelperLibrary.Utilities;
using SchoolManagementSystem.Service;
using SchoolManagementSystem.Service.BusinessLogic;
using SchoolManagementSystem.Service.BusinessLogic.Auth;
using SchoolManagementSystem.Service.Display;
using SchoolManagementSystem.Service.DTOs.User.Auth;

namespace SchoolManagementSystem;

public static class Menu
{
    private static readonly RepositoryFactory Repos = new ();
    private static readonly UserService UserService = new(Repos);
    private static readonly AuthService AuthService = new();
    public static async Task Run()
    {
        await Initializer.Execute();

        // test
        var loginRequestDTO = new LoginDTO
        {
            Email = "superadmin@gmail.com",
            PrivateId = null,
            Password = "admin123"
        };
        var loginResponse = await AuthService.Login(loginRequestDTO);
        if (loginResponse.Success)
        {
            Console.WriteLine("Login successful");
            DisplayManager.Print(loginResponse.Value, " - ");
        }
        else
        {
            Console.WriteLine($"Log in failed. {loginResponse.Message}");
        }
        
        var user = new AdminRegisterDTO
        {
            FirstName = "meore sandro",
            LastName = "benashvili",
            Address = "yvelgan",
            Email = "abg@gmail.com",
            DateOfBirth = new DateTime(1980, 1, 1),
            Password = "123456",
            PhoneNumber = "4343",
            PrivateId = "esec nagdi",
            RoleId = 4,
            
        };

        var student = new StudentRegisterDTO
        {
            FirstName = "student",
            LastName = "student but last name",
            Address = "malawi",
            Email = "apk@gmail.com",
            DateOfBirth = new DateTime(2001, 4, 19),
            Password = "securePassword",
            PhoneNumber = "454343",
            PrivateId = "nagdiId",
        };

        var userRegisterResponse = await AuthService.RegisterUser(user);
        var studentRegisterResponse = await AuthService.RegisterStudent(student);
        
        ConsoleUtilities.ResetMenu();
        
        if (userRegisterResponse.Success)
        {
            var registeredUserResponse = await UserService.GetUserByEmail(user.Email);
            if (registeredUserResponse.Success)
            {
                var registeredUser = registeredUserResponse.Value;
                Console.WriteLine($"User {registeredUser.FullName} successfully registered. (privateId: {registeredUser.PrivateId})");
            }
            else
            {
                Console.WriteLine(registeredUserResponse.Message);
            }
        }
        else
        {
            Console.WriteLine(userRegisterResponse.Message);
        }
        
        ConsoleUtilities.ResetMenu();
        
        if (studentRegisterResponse.Success)
        {
            var registeredUserResponse = await UserService.GetUserByEmail(student.Email);
            if (registeredUserResponse.Success)
            {
                var registeredStudent = registeredUserResponse.Value;
                Console.WriteLine(
                    $"Student - {registeredStudent.FullName} - successfully registered. PrivateId: {registeredStudent.PrivateId}");
            }
            else
            {
                Console.WriteLine(registeredUserResponse.Message);
            }
        }
        else
        {
            Console.WriteLine(studentRegisterResponse.Message);
        }


        // var getUserResponse = await UserService.GetUserById(3);
        // if (getUserResponse.Success)
        // {
        //     Console.WriteLine("User successfully retrieved");
        //     DisplayManager.Print(getUserResponse.Value, " - ");
        // }
        //
        ConsoleUtilities.ResetMenu(userMessage: "Press any key to exit menu...");
    }
}