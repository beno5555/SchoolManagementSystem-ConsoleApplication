using SchoolManagementSystem.Service.BusinessLogic.Utilities;

namespace SchoolManagementSystem.Service.BusinessLogic.Factories;

public class UtilityFactory
{
    internal readonly RepositoryFactory Repos;

    
    private readonly Lazy<IdentityService> _identityServiceFactory;
    private readonly Lazy<MapperService> _mapperFactory;
    
    private readonly Lazy<PasswordHasher> _passwordHasherFactory = new(() => new PasswordHasher());
    
    public UtilityFactory(RepositoryFactory repos)
    {
        Repos = repos;
        _mapperFactory = new Lazy<MapperService>(() => new MapperService(repos, _passwordHasherFactory.Value));
        _identityServiceFactory = new Lazy<IdentityService>(() => new IdentityService(
            Repos,
            _mapperFactory.Value,
            _passwordHasherFactory.Value));
        
    }
    
    public PasswordHasher PasswordHasher => _passwordHasherFactory.Value;
    public IdentityService IdentityService => _identityServiceFactory.Value;
    public MapperService MapperService => _mapperFactory.Value;

}