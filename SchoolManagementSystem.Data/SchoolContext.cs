using SchoolManagementSystem.Data.Constants;
using SchoolManagementSystem.Data.HelperClasses;
using SchoolManagementSystem.Data.Models;
using SchoolManagementSystem.Data.Models.JoinedModels;
using SchoolManagementSystem.Data.Models.UserProfiles;

namespace SchoolManagementSystem.Data;

public class SchoolContext
{
    #region Properties
    
    #region Entities
    
    // main    
    public List<User> Users { get; set; } = new(1000);
    public List<Subject> Subjects { get; set; } = new(50);
    public List<Room> Rooms { get; set; } = new(150);
    public List<Role> Roles { get; set; } = new(10);
    public List<Permission> Permissions { get; set; } = new(50);
    public List<Laboratory> Laboratories { get; set; } = new(5);
    public List<Group> Groups { get; set; } = new(100);

    // profiles    
    public List<StudentProfile> StudentProfiles { get; set; } = new();
    public List<TeacherProfile> TeacherProfiles { get; set; } = new();
    public List<PrincipalProfile> PrincipalProfiles { get; set; } = new(1);

    // joined    
    public List<TeacherSubject> TeacherSubjects { get; set; } = new();
    public List<SubjectEnrollment> SubjectEnrollments { get; set; } = new(10_000); // need to specify capacity
    public List<RolePermission> RolePermissions { get; set; } = new(50);
    
    // academic    
    public List<Assignment> Assignments { get; set; } = new();
    public List<Assessment> Assessments { get; set; } = new();
    public List<AssignmentType> AssignmentTypes { get; set; } = new(10);
    
    #endregion

    #endregion
    
    #region Constructors



    #endregion
    
    #region Methods
    
    #region After Initialization
    
    public async Task InitializeAsync()
    {
        await ForceStorage();
        await SeedData();
    }
    private async Task ForceStorage()
    {
        Directory.CreateDirectory(FolderPaths.DataPath);

        foreach (string jsonPath in FolderPaths.JsonPaths)
        {
            await FileManager.ForceFile(jsonPath);
        }
    }

    #endregion

    #region Seeding

    public async Task SeedData()
    {
        Seeder.SeedEnums(Roles, typeof(SchoolEnums.RoleName), name => new Role(name));
        Seeder.SeedEnums(Permissions, typeof(SchoolEnums.Permission), name => new Permission(name));
        Seeder.SeedEnums(Subjects, typeof(SchoolEnums.SubjectName), name => new Subject(name));
        Seeder.SeedEnums(AssignmentTypes, typeof(SchoolEnums.AssignmentType), name => new AssignmentType(name));
        
        Seeder.SeedSuperAdmin(Users, Roles);

        await SaveSeededData();
    }

    public async Task SaveSeededData()
    {
        await FileManager.SaveAsync(FolderPaths.GetFullPath(FolderPaths.RolePath), Roles);
        await FileManager.SaveAsync(FolderPaths.GetFullPath(FolderPaths.PermissionPath), Permissions);
        await FileManager.SaveAsync(FolderPaths.GetFullPath(FolderPaths.SubjectPath), Subjects);
        await FileManager.SaveAsync(FolderPaths.GetFullPath(FolderPaths.AssignmentTypePath), AssignmentTypes);
        await FileManager.SaveAsync(FolderPaths.GetFullPath(FolderPaths.UserPath), Users);
    }
    
    #endregion
    
    #region Rubbish

    //public async Task LoadData()
    //{
    //    Users = await LoadAsync<User>(UserPath);
    //    Subjects = await LoadAsync<Subject>(SubjectPath);
    //    Rooms = await LoadAsync<Room>(RoomPath);
    //    Roles = await LoadAsync<Role>(RolePath);
    //    Permissions = await LoadAsync<Permission>(PermissionPath);
    //    Laboratories = await LoadAsync<Laboratory>(LaboratoryPath);
    //    Groups = await LoadAsync<Group>(GroupPath);

    //    Assignments = await LoadAsync<Assignment>(AssignmentPath);
    //    Assessments = await LoadAsync<Assessment>(AssessmentPath);

    //    TeacherProfiles = await LoadAsync<TeacherProfile>(TeacherProfilePath);
    //    StudentProfiles = await LoadAsync<StudentProfile>(StudentProfilePath);
    //    PrincipalProfiles = await LoadAsync<PrincipalProfile>(PrincipalProfilePath);

    //    TeacherSubjects = await LoadAsync<TeacherSubject>(TeacherSubjectPath);
    //    SubjectEnrollments = await LoadAsync<SubjectEnrollment>(SubjectEnrollmentPath);
    //    RolePermissions = await LoadAsync<RolePermission>(RolePermissionPath);

    //}

    //public async Task SaveData()
    //{
    //    await SaveAsync(UserPath, Users);
    //    // ..
    //}

    #endregion

    #endregion
}
