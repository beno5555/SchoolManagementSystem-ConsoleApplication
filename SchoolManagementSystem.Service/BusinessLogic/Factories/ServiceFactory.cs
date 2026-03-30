using SchoolManagementSystem.Service.BusinessLogic.Services;

namespace SchoolManagementSystem.Service.BusinessLogic.Factories;

public class ServiceFactory
{
    
    private readonly Lazy<AuthService> _authServiceFactory;
    private readonly Lazy<StudentService> _studentServiceFactory;
    private readonly Lazy<PermissionService> _permissionServiceFactory;
    private readonly Lazy<UserService> _userServiceFactory;
    private readonly Lazy<RoomService> _roomServiceFactory;
    
    
    public ServiceFactory(UtilityFactory utilities)
    {
        _authServiceFactory = new Lazy<AuthService>(() => new AuthService(utilities));
        _studentServiceFactory = new Lazy<StudentService>(() => new StudentService(utilities));
        _permissionServiceFactory = new Lazy<PermissionService>(() => new PermissionService(utilities));
        _userServiceFactory = new Lazy<UserService>(() => new UserService(utilities));
        _roomServiceFactory = new Lazy<RoomService>(() => new RoomService(utilities));
    }
    
    public AuthService AuthService => _authServiceFactory.Value;
    public StudentService StudentService => _studentServiceFactory.Value;
    public PermissionService PermissionService => _permissionServiceFactory.Value;
    public UserService UserService => _userServiceFactory.Value;
    public RoomService RoomService => _roomServiceFactory.Value;
    
}