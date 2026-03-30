using SchoolManagementSystem.ConsoleDisplay;
using SchoolManagementSystem.Controllers;
using SchoolManagementSystem.DTOCreation;
using SchoolManagementSystem.Service;
using SchoolManagementSystem.Service.BusinessLogic.Factories;
using SchoolManagementSystem.Service.DTOs.User.Auth;

namespace SchoolManagementSystem.Menus;

public static class Menu
{
    private static SessionUser? _sessionUser;
    private static readonly RepositoryFactory Repos = new ();
    private static readonly UtilityFactory Utilities = new (Repos);
    
    private static readonly ServiceFactory Services = new(Utilities);
    private static readonly ControllerFactory Controllers = new(Services);
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
                Controllers.ClearSession();
            }
            else
                await CompleteAction(action);
        }
    }

    private static async Task CompleteAction(string action)
    {   
        if (_sessionUser is null) return;
        
        // temporarily disregarding permission checking for simplicity's sake
        bool hasPermission = await Services.PermissionService.HasPermission(action, _sessionUser.Role);

        switch (_sessionUser.Role)
        {
            case nameof(MenuConstants.Roles.Student):
                await CompleteStudentAction(action);
                break;
            case nameof(MenuConstants.Roles.Teacher):
                await CompleteTeacherAction(action);
                break;
            case nameof(MenuConstants.Roles.Principal):
                await CompletePrincipalAction(action);
                break;
            case nameof(MenuConstants.Roles.SuperAdmin):
                await CompleteSuperAdminAction(action); 
                break;
        }
    }

    private static async Task CompleteStudentAction(string action)
    {
        switch (action)
        {
            case nameof(MenuConstants.ActionEnums.StudentAction.ViewMyGrades):
                await Controllers.StudentController.ViewMyGrades();
                break;
            case nameof(MenuConstants.ActionEnums.StudentAction.Subjects):
                await Controllers.StudentController.ViewMySubjects();
                break;
            case nameof(MenuConstants.ActionEnums.StudentAction.Assignments):
                await Controllers.StudentController.ViewAssignments();
                break;
            case nameof(MenuConstants.ActionEnums.StudentAction.Teachers):
                await Controllers.StudentController.ViewMyTeachers();
                break;
        }
    }
    

    private static async Task CompleteTeacherAction(string action)
    {
        switch (action)
        {
            case nameof(MenuConstants.ActionEnums.TeacherAction.Assignments):
                await Controllers.TeacherController.UploadAssignment();
                break;
            case nameof(MenuConstants.ActionEnums.TeacherAction.Assessment):
                await Controllers.TeacherController.WriteAssessment();
                break;
        }
    }

    private static async Task CompletePrincipalAction(string action)
    {
        
    }

    private static async Task CompleteSuperAdminAction(string action)
    {
        switch (action)
        {
            case nameof(MenuConstants.ActionEnums.SuperAdminAction.RegisterUser):
                await Controllers.SuperAdminController.RegisterUser();
                break;
            case nameof(MenuConstants.ActionEnums.SuperAdminAction.AssignSubjectToTeacher):
                await Controllers.SuperAdminController.AssignSubjectToTeacher();
                break;
            case nameof(MenuConstants.ActionEnums.SuperAdminAction.Groups):
                await Controllers.SuperAdminController.ManageGroups();
                break;
            case nameof(MenuConstants.ActionEnums.SuperAdminAction.Rooms):
                await Controllers.SuperAdminController.ManageRooms();
                break;
        } 
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
            Controllers.SetUser(_sessionUser);
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

        var studentRegisterDTO = (StudentRegisterDTO)UserDTOCreation.PromptBaseRegisterDTO();
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

    }
}