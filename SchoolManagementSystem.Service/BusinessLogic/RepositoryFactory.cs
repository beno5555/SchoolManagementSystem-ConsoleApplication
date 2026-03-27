using SchoolManagementSystem.Data.Repositories;
using SchoolManagementSystem.Service.Mapping;

namespace SchoolManagementSystem.Service.BusinessLogic;

public class RepositoryFactory
{
    private readonly Lazy<UserRepository> _userRepositoryFactory = new(() => new UserRepository());
    private readonly Lazy<SubjectRepository>  _subjectRepositoryFactory = new(() => new SubjectRepository());
    private readonly Lazy<SubjectEnrollmentRepository> _subjectEnrollmentRepositoryFactory = new(() => new SubjectEnrollmentRepository());
    private readonly Lazy<SchoolClassRepository> _schoolClassRepositoryFactory = new(() => new SchoolClassRepository());
    private readonly Lazy<RoomRepository> _roomRepositoryFactory = new(() => new RoomRepository());
    private readonly Lazy<RoleRepository> _roleRepositoryFactory = new(() => new RoleRepository());
    private readonly Lazy<PermissionRepository> _permissionRepositoryFactory = new(() => new PermissionRepository());
    private readonly Lazy<GroupRepository> _groupRepositoryFactory = new(() => new GroupRepository());
    private readonly Lazy<AssignmentRepository> _assignmentRepositoryFactory = new(() => new AssignmentRepository());
    private readonly Lazy<AssessmentRepository> _assessmentRepositoryFactory = new(() => new AssessmentRepository());

    private readonly Lazy<Mapper> _mapperFactory = new(() => new Mapper());
    
    
    
    public UserRepository UserRepository => _userRepositoryFactory.Value;
    public SubjectRepository SubjectRepository => _subjectRepositoryFactory.Value;
    public SubjectEnrollmentRepository SubjectEnrollmentRepository => _subjectEnrollmentRepositoryFactory.Value;
    public SchoolClassRepository SchoolClassRepository => _schoolClassRepositoryFactory.Value;
    public RoomRepository RoomRepository => _roomRepositoryFactory.Value;
    public RoleRepository RoleRepository => _roleRepositoryFactory.Value;
    public PermissionRepository PermissionRepository => _permissionRepositoryFactory.Value;
    public GroupRepository GroupRepository => _groupRepositoryFactory.Value;
    public AssignmentRepository AssignmentRepository => _assignmentRepositoryFactory.Value;
    public AssessmentRepository AssessmentRepository => _assessmentRepositoryFactory.Value;
    
    public Mapper Mapper => _mapperFactory.Value;
}