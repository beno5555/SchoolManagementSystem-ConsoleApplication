using ProjectHelperLibrary.Utilities;
using SchoolManagementSystem.ConsoleDisplay;
using SchoolManagementSystem.Service;
using SchoolManagementSystem.Service.BusinessLogic.Factories;
using SchoolManagementSystem.Service.DTOs.User.Auth;

namespace SchoolManagementSystem.Menus;

public static class Menu
{
    private static readonly RepositoryFactory Repos = new ();
    private static readonly UtilityFactory Utilities = new (Repos);
    private static SessionUser? _sessionUser;
    
    private static readonly ServiceFactory Services = new(Utilities);
    public static async Task Run()
    {
        await Initializer.Execute();
        
        bool shouldRun = true;
        while (shouldRun)
        {
            var action = UnauthenticatedMenu.Run();

            switch (action)
            {
                case UnauthenticatedAction.SignIn:
                    await RunAuthenticatedSession();
                    break;
                case UnauthenticatedAction.SignUp:
                    await SignUp();
                    break;
                case  UnauthenticatedAction.Exit:
                    shouldRun = false;
                    break;
            }
        }
    }

    private static async Task RunAuthenticatedSession()
    {
        await SignIn();
        while (_sessionUser is not null)
        {
            var action = AuthenticatedMenu.Run(_sessionUser.FullName, _sessionUser.Role);

            if (action.Equals(nameof(MenuConstants.ActionEnums.SharedAction.Logout), StringComparison.OrdinalIgnoreCase))
            {
                _sessionUser = null;
            }
            else
                await CompleteAction(action);
        }
    }

    private static async Task CompleteAction(string action)
    {   
        if (_sessionUser is null) return;
        bool hasPermission = await Services.PermissionService.HasPermission(action, _sessionUser.Role);

        if (hasPermission)
        {
            switch (_sessionUser.Role)
            {
                case MenuConstants.Roles.Student:
                    await CompleteStudentAction(action);
                    break;
                case MenuConstants.Roles.Teacher:
                    await CompleteTeacherAction(action);
                    break;
                case MenuConstants.Roles.Principal:
                    await CompletePrincipalAction(action);
                    break;
                case MenuConstants.Roles.SuperAdmin:
                    await CompleteSuperAdminAction(action); 
                    break;
            }
        }
    }

    private static async Task CompleteStudentAction(string action)
    {
        
    }

    private static async Task CompleteTeacherAction(string action)
    {
        
    }

    private static async Task CompletePrincipalAction(string action)
    {
        
    }

    private static async Task CompleteSuperAdminAction(string action)
    {
        
    }

    private static async Task SignIn()
    {
        LayoutHelper.RenderWelcomeScreen();
        LayoutHelper.RenderSectionTitle("Sign In");

        var loginDTO = new LoginDTO
        {
            Identifier    = LayoutHelper.GetInput("Email or PrivateId"),
            Password = LayoutHelper.GetInput("Password", secret: true)
        };

        var result = await Services.AuthService.SignIn(loginDTO);

        if (result.Success)
        {
            LayoutHelper.ShowSuccess("You are signed in!");
            _sessionUser = result.Value;
        }
        else
        {
            LayoutHelper.ShowError("Login failed.");
        } 
    }
    private static async Task SignUp()
    {
        LayoutHelper.RenderWelcomeScreen();
        LayoutHelper.RenderSectionTitle("Sign Up");

        var studentRegisterDTO = new StudentRegisterDTO
        {
            FirstName = LayoutHelper.GetInput("First Name"),
            LastName = LayoutHelper.GetInput("Last Name"),
            PhoneNumber = LayoutHelper.GetInput("Phone Number"),
            Address = LayoutHelper.GetInput("Address"),
            PrivateId = LayoutHelper.GetInput("National Id number"),
            Email = LayoutHelper.GetInput("Email"),
            DateOfBirth = LayoutHelper.GetDateInput("Birth Date"),
            Password = LayoutHelper.GetInput("Password", secret: true)
        };
        var signUpResponse = await Services.AuthService.RegisterStudent(studentRegisterDTO);

        if (signUpResponse.Success)
        {
            LayoutHelper.ShowSuccess("Registration Successful. You can now sign in.");
        }
        else
        {
            LayoutHelper.ShowError(signUpResponse.Message);
        }
    }
   
    // test
    private static async Task Test()
    {
        var loginRequestDTO = new LoginDTO
        {
            Identifier = "superadmin@gmail.com",
            Password = "admin123"
        };
        var loginResponse = await Services.AuthService.SignIn(loginRequestDTO);
        if (loginResponse.Success)
        {
            Console.WriteLine("SignIn successful");
            // DisplayManager.Print(loginResponse.Value, " - ");
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

        var userRegisterResponse = await Services.AuthService.RegisterUser(user);
        var studentRegisterResponse = await Services.AuthService.RegisterStudent(student);
        
        ConsoleUtilities.ResetMenu();
        
        if (userRegisterResponse.Success)
        {
            var registeredUserResponse = await Services.StudentService.GetUserByEmail(user.Email);
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
            var registeredUserResponse = await Services.StudentService.GetUserByEmail(student.Email);
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


        // var getUserResponse = await Services.StudentService.GetUserById(3);
        // if (getUserResponse.Success)
        // {
        //     Console.WriteLine("User successfully retrieved");
        //     DisplayManager.Print(getUserResponse.Value, " - ");
        // }
        //
        ConsoleUtilities.ResetMenu(userMessage: "Press any key to exit menu...");
    }
}