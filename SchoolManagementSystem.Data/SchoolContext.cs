using SchoolManagementSystem.Data.Config;
using SchoolManagementSystem.Data.HelperClasses;
using SchoolManagementSystem.Data.Models;
using SchoolManagementSystem.Data.Models.JoinedModels;

namespace SchoolManagementSystem.Data;

public class SchoolContext
{
    #region Properties
    
    // main    
    public List<User> Users { get; set; } = new(AppConstants.MaximumCount.Users);
    public List<Subject> Subjects { get; set; } = new(AppConstants.MaximumCount.Subjects);
    public List<Room> Rooms { get; set; } = new(AppConstants.MaximumCount.Rooms);
    public List<Role> Roles { get; set; } = new(AppConstants.MaximumCount.Roles);
    public List<Permission> Permissions { get; set; } = new(AppConstants.MaximumCount.Permissions);
    public List<Group> Groups { get; set; } = new(AppConstants.MaximumCount.Groups);
    
    // academic    
    public List<Assignment> Assignments { get; set; } = new(AppConstants.MaximumCount.Assignments);
    public List<Assessment> Assessments { get; set; } = new(AppConstants.MaximumCount.Assessments);
    public List<AssignmentType> AssignmentTypes { get; set; } = new(AppConstants.MaximumCount.AssignmentTypes);
    
    // joined    
    public List<TeacherSubject> TeacherSubjects { get; set; } = new(AppConstants.MaximumCount.TeacherSubjects);
    public List<SubjectEnrollment> SubjectEnrollments { get; set; } = new(AppConstants.MaximumCount.SubjectEnrollments); 
    public List<RolePermission> RolePermissions { get; set; } = new(AppConstants.MaximumCount.RolePermissions);
    
    #endregion
    
    #region Methods
    
    #region Initialization
    
    public async Task InitializeAsync()
    {
        await FileManager.ForceStorage();
        await SeedData();
        InitializeIds();
    }

    // using for testing. might remove later
    private void InitializeIds()
    {
        IdGenerator.InitializeId(Users);
        IdGenerator.InitializeId(Subjects);
        IdGenerator.InitializeId(Rooms);
        IdGenerator.InitializeId(Roles);
        IdGenerator.InitializeId(Permissions);
        IdGenerator.InitializeId(Groups);
        
        // IdGenerator.InitializeId(StudentProfiles);
        // IdGenerator.InitializeId(TeacherProfiles);
        // IdGenerator.InitializeId(PrincipalProfiles);
        
        IdGenerator.InitializeId(Assessments);
        IdGenerator.InitializeId(AssignmentTypes);
        IdGenerator.InitializeId(Assignments);
    }

    #endregion

    #region Seeding

    public async Task SeedData()
    {
        await LoadEssentialCollections();
        
        Seeder.SeedEnums(Roles, typeof(SchoolEnums.RoleName), name => new Role(name));
        Seeder.SeedEnums(Permissions, typeof(SchoolEnums.Permission), name => new Permission(name));
        Seeder.SeedEnums(Subjects, typeof(SchoolEnums.SubjectName), name => new Subject(name));
        Seeder.SeedEnums(AssignmentTypes, typeof(SchoolEnums.AssignmentType), name => new AssignmentType(name));
        
        Seeder.SeedSuperAdmin(Users, Roles);

        await SaveSeededData();
    }

    private async Task LoadEssentialCollections()
    {
        await FileManager.LoadAsync(AppConstants.FolderPaths.RolePath, Roles);
        await FileManager.LoadAsync(AppConstants.FolderPaths.PermissionPath, Permissions);
        await FileManager.LoadAsync(AppConstants.FolderPaths.SubjectPath, Subjects);
        await FileManager.LoadAsync(AppConstants.FolderPaths.AssignmentTypePath, AssignmentTypes);
        await FileManager.LoadAsync(AppConstants.FolderPaths.UserPath, Users);
    }

    public async Task SaveSeededData()
    {
        
        await FileManager.SaveAsync(AppConstants.FolderPaths.RolePath, Roles);
        await FileManager.SaveAsync(AppConstants.FolderPaths.PermissionPath, Permissions);
        await FileManager.SaveAsync(AppConstants.FolderPaths.SubjectPath, Subjects);
        await FileManager.SaveAsync(AppConstants.FolderPaths.AssignmentTypePath, AssignmentTypes);
        await FileManager.SaveAsync(AppConstants.FolderPaths.UserPath, Users);
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

    
    // public void Try()
    // {
    //     int? studentRoleId = Roles.FirstOrDefault(role => role.RoleName == nameof(SchoolEnums.RoleName.Student))?.Id;
    //     if (studentRoleId is not null)
    //     {
    //         int finalStudentGrade = 0;
    //         var student = Users.FirstOrDefault(user => user.RoleId == studentRoleId.Value);
    //         var studentSubjectEnrollments = SubjectEnrollments.Where(enrollment => enrollment.StudentId == student.Id ).ToList();
    //         var finalGrade = studentSubjectEnrollments.Average(sse => sse.GetAverageGrade());
    //         finalStudentGrade = (int)Math.Round(finalGrade);
    //     }
    //     
    // }

    #endregion

    #endregion
}
