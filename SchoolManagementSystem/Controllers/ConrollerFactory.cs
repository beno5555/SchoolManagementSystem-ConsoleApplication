using SchoolManagementSystem.Service.BusinessLogic.Factories;
using SchoolManagementSystem.Service.DTOs.User.Auth;

namespace SchoolManagementSystem.Controllers;

public class ControllerFactory
{
    private readonly Lazy<StudentController> _studentController;

    public ControllerFactory(ServiceFactory services, SessionUser? user)
    {
        _studentController = new Lazy<StudentController>(() => new StudentController(services, user));
        // ..
    }
    
    public StudentController StudentController => _studentController.Value;


    public void SetUser(SessionUser? sessionUser)
    {
        StudentController.SetUser(sessionUser);
        // ..
    }

    public void ClearSession()
    {
        StudentController.ClearSession();
        // ..
    }
}