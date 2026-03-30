using SchoolManagementSystem.Service.BusinessLogic.Factories;
using SchoolManagementSystem.Service.DTOs.User.Auth;

namespace SchoolManagementSystem.Controllers;

public class ControllerFactory
{
    private readonly Lazy<StudentController> _studentController;
    private readonly Lazy<TeacherController> _teacherController;
    private readonly Lazy<SuperAdminController> _superAdminController;

    public ControllerFactory(ServiceFactory services)
    {
        _studentController = new Lazy<StudentController>(() => new StudentController(services));
        _teacherController = new Lazy<TeacherController>(() => new TeacherController(services));
        _superAdminController = new Lazy<SuperAdminController>(() => new SuperAdminController(services));
    }
    
    public StudentController StudentController => _studentController.Value;
    public TeacherController TeacherController => _teacherController.Value;
    public SuperAdminController SuperAdminController => _superAdminController.Value;

    public void SetUser(SessionUser sessionUser)
    {
        StudentController.SetUser(sessionUser);
        TeacherController.SetUser(sessionUser);
        SuperAdminController.SetUser(sessionUser);
    }

    public void ClearSession()
    {
        StudentController.ClearSession();
        TeacherController.ClearSession();
        SuperAdminController.ClearSession();
    }
}