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
    
    // joined    
    public List<SubjectEnrollment> SubjectEnrollments { get; set; } = new(AppConstants.MaximumCount.SubjectEnrollments); 
    public List<RolePermission> RolePermissions { get; set; } = new(AppConstants.MaximumCount.RolePermissions);
    public List<Class> Classes { get; set; } = new();

    // types
    public List<RoomType> RoomTypes { get; set; } = new();
    public List<AssignmentType> AssignmentTypes { get; set; } = new(AppConstants.MaximumCount.AssignmentTypes);

    #endregion
    
    #region Methods
    
    #region Initialization
    
    public async Task InitializeAsync()
    {
        await FileManager.EnsureFoldersExist();
        await SeedData();
        await InitializeIds();
    }

    // using for testing. might remove later
    private async Task InitializeIds()
    {
        await IdGenerator.InitializeId(Users);
        await IdGenerator.InitializeId(Subjects);
        await IdGenerator.InitializeId(Rooms);
        await IdGenerator.InitializeId(Roles);
        await IdGenerator.InitializeId(Permissions);
        await IdGenerator.InitializeId(Groups);
        
        await IdGenerator.InitializeId(Assessments);
        await IdGenerator.InitializeId(Assignments);
        
        await IdGenerator.InitializeId(SubjectEnrollments);
        await IdGenerator.InitializeId(Classes);
        
        await IdGenerator.InitializeId(AssignmentTypes);
        await IdGenerator.InitializeId(RoomTypes);
    }

    #endregion

    #region Seeding

    public async Task SeedData()
    {
        await Seeder.SeedEnums(Roles, typeof(SchoolEnums.RoleName), name => new Role(name));
        await Seeder.SeedEnums(Permissions, typeof(SchoolEnums.PermissionName), name => new Permission(name));
        await Seeder.SeedEnums(Subjects, typeof(SchoolEnums.SubjectName), name => new Subject(name));
        await Seeder.SeedEnums(AssignmentTypes, typeof(SchoolEnums.AssignmentTypeName), name => new AssignmentType(name));
        await Seeder.SeedEnums(RoomTypes, typeof(SchoolEnums.RoomTypeName), name => new RoomType(name));
        
        await Seeder.SeedSuperAdmin(Users, Roles);
    }
    
    #endregion
    
    #region Rubbish

    //public async Task LoadData()
    //{
    //    Users = await LoadAsync<User>(UserPath);
    //    Subjects = await LoadAsync<Subject>(SubjectPath);
    //    Rooms = await LoadAsync<Room>(RoomPath);
    //    Roles = await LoadAsync<Role>(RolePath);
    //    Permissions = await LoadAsync<PermissionName>(PermissionPath);
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
