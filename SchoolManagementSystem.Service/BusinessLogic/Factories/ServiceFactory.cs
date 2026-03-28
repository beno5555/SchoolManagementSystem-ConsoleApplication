using SchoolManagementSystem.Service.BusinessLogic.Services;

namespace SchoolManagementSystem.Service.BusinessLogic.Factories;

public class ServiceFactory
{
    
    private readonly Lazy<AuthService> _authServiceFactory;
    private readonly Lazy<StudentService> _userServiceFactory;
    
    public ServiceFactory(UtilityFactory utilities)
    {
        _authServiceFactory = new Lazy<AuthService>(() => new AuthService(utilities));
        _userServiceFactory = new Lazy<StudentService>(() => new StudentService(utilities));
    }
    
    public AuthService AuthService => _authServiceFactory.Value;
    public StudentService StudentService => _userServiceFactory.Value;
    
}