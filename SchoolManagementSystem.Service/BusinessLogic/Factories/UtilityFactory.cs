using SchoolManagementSystem.Service.BusinessLogic.Utilities;

namespace SchoolManagementSystem.Service.BusinessLogic.Factories;

public class UtilityFactory
{
    internal readonly RepositoryFactory Repos;

    
    private readonly Lazy<IdentityService> _identityServiceFactory;
    private readonly Lazy<AcademicService> _academicServiceFactory;
    private readonly Lazy<MapperService> _mapperFactory;
    private readonly Lazy<MethodHelper> _methodHelperFactory = new(() => new MethodHelper());
    private readonly Lazy<PasswordHasher> _passwordHasherFactory = new(() => new PasswordHasher());
    
    public UtilityFactory(RepositoryFactory repos)
    {
        Repos = repos;
        
        _mapperFactory = new Lazy<MapperService>(() => new MapperService(
            repos, 
            _methodHelperFactory.Value,
            _passwordHasherFactory.Value));
        
        _identityServiceFactory = new Lazy<IdentityService>(() => new IdentityService(
            Repos,
            _mapperFactory.Value,
            _passwordHasherFactory.Value));
        
        _academicServiceFactory = new Lazy<AcademicService>(() => new AcademicService(
            Repos, 
            _mapperFactory.Value,
            _methodHelperFactory.Value));
        
    }
    
    public IdentityService IdentityService => _identityServiceFactory.Value;
    public AcademicService AcademicService => _academicServiceFactory.Value;
    public MapperService MapperService => _mapperFactory.Value;
    public MethodHelper MethodHelper => _methodHelperFactory.Value;
    public PasswordHasher PasswordHasher => _passwordHasherFactory.Value;

}