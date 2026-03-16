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
    
    public async Task OnInitializedAsync()
    {
        await ForceStorage();
        SeedData();
    }
    private async Task ForceStorage()
    {
        Directory.CreateDirectory(FolderPaths.DataFolder);
        
        await FileManager.ForceFile(FolderPaths.UserPath);
        await FileManager.ForceFile(FolderPaths.SubjectPath);
        await FileManager.ForceFile(FolderPaths.RoomPath);
        await FileManager.ForceFile(FolderPaths.RolePath);
        await FileManager.ForceFile(FolderPaths.PermissionPath);
        await FileManager.ForceFile(FolderPaths.LaboratoryPath);
        await FileManager.ForceFile(FolderPaths.GroupPath);
        
        await FileManager.ForceFile(FolderPaths.AssignmentPath);
        await FileManager.ForceFile(FolderPaths.AssessmentPath);
        await FileManager.ForceFile(FolderPaths.AssignmentTypePath);
        await FileManager.ForceFile(FolderPaths.TeacherProfilePath);
        await FileManager.ForceFile(FolderPaths.StudentProfilePath);
        await FileManager.ForceFile(FolderPaths.PrincipalProfilePath);

        await FileManager.ForceFile(FolderPaths.TeacherSubjectPath);
        await FileManager.ForceFile(FolderPaths.SubjectEnrollmentPath);
        await FileManager.ForceFile(FolderPaths.RolePermissionPath);

    }

    #endregion

    #region Seeding

    public void SeedData()
    {
        Seeder.SeedEnums(Roles, typeof(SchoolEnums.RoleName), (id, name) => new Role(id, name));
        Seeder.SeedEnums(Permissions, typeof(SchoolEnums.Permission), (id, name) => new Permission(id, name));
        Seeder.SeedEnums(Subjects, typeof(SchoolEnums.SubjectName), (id, name) => new Subject(id, name));
        Seeder.SeedEnums(AssignmentTypes, typeof(SchoolEnums.AssignmentType), (id, name) => new AssignmentType(id, name));
        
        Seeder.SeedSuperAdmin(Users, Roles);
    }
    
    #endregion
    
    #region Rubbish

    //public async Task LoadData()
    //{
    //    Users = await Load<User>(UserPath);
    //    Subjects = await Load<Subject>(SubjectPath);
    //    Rooms = await Load<Room>(RoomPath);
    //    Roles = await Load<Role>(RolePath);
    //    Permissions = await Load<Permission>(PermissionPath);
    //    Laboratories = await Load<Laboratory>(LaboratoryPath);
    //    Groups = await Load<Group>(GroupPath);

    //    Assignments = await Load<Assignment>(AssignmentPath);
    //    Assessments = await Load<Assessment>(AssessmentPath);

    //    TeacherProfiles = await Load<TeacherProfile>(TeacherProfilePath);
    //    StudentProfiles = await Load<StudentProfile>(StudentProfilePath);
    //    PrincipalProfiles = await Load<PrincipalProfile>(PrincipalProfilePath);

    //    TeacherSubjects = await Load<TeacherSubject>(TeacherSubjectPath);
    //    SubjectEnrollments = await Load<SubjectEnrollment>(SubjectEnrollmentPath);
    //    RolePermissions = await Load<RolePermission>(RolePermissionPath);

    //}

    //public async Task SaveData()
    //{
    //    await Save(UserPath, Users);
    //    // ..
    //}

    #endregion

    #endregion
}
