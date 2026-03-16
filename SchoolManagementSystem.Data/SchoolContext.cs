using System.Text.Json;
using SchoolManagementSystem.Data.Constants;
using SchoolManagementSystem.Data.Models;
using SchoolManagementSystem.Data.Models.JoinedModels;
using SchoolManagementSystem.Data.Models.UserProfiles;

namespace SchoolManagementSystem.Data;

public class SchoolContext
{
    #region Properties
    
    #region Entities
    
    #region Main
    
    public List<User> Users { get; set; } = new(1000);
    public List<Subject> Subjects { get; set; } = new(50);
    public List<Room> Rooms { get; set; } = new(150);
    public List<Role> Roles { get; set; } = new(10);
    public List<Permission> Permissions { get; set; } = new(50);
    public List<Laboratory> Laboratories { get; set; } = new(5);
    public List<Group> Groups { get; set; } = new(100);

    #endregion
    
    #region Profiles
    
    public List<StudentProfile> StudentProfiles { get; set; } = new();
    public List<TeacherProfile> TeacherProfiles { get; set; } = new();
    public List<PrincipalProfile> PrincipalProfiles { get; set; } = new(1);

    #endregion
    
    #region Joined
    
    public List<TeacherSubject> TeacherSubjects { get; set; } = new();
    public List<SubjectEnrollment> SubjectEnrollments { get; set; } = new(10_000); // need to specify capacity
    public List<RolePermission> RolePermissions { get; set; } = new(50);
    
    #endregion
    
    #region Academic
    
    public List<Assignment> Assignments { get; set; } = new();
    public List<Assessment> Assessments { get; set; } = new();
    
    #endregion
    
    #endregion

    #region File Paths

    private const string DataFolder = "DataFolder";
    //private const string IdCounterPath = "idCounters.json";
    
    #region Main
    
    private const string UserPath = "users.json";
    
    private const string SubjectPath = "subjects.json";
    private const string RoomPath = "rooms.json";
    private const string RolePath = "roles.json";
    private const string PermissionPath = "permissions.json";
    private const string LaboratoryPath = "laboratories.json";
    private const string GroupPath = "groups.json";
    
    #endregion
    
    #region Academic
    
    private const string AssignmentPath = "assignments.json";
    private const string AssessmentPath = "assessments.json";
    
    #endregion
    
    #region Profiles
    
    private const string TeacherProfilePath = "teacherProfiles.json";
    private const string StudentProfilePath = "studentProfiles.json";
    private const string PrincipalProfilePath = "principalProfiles.json";

    #endregion
    
    #region Joined
    
    private const string TeacherSubjectPath = "teacherSubjects.json";
    private const string SubjectEnrollmentPath = "subjectEnrollments.json";
    private const string RolePermissionPath = "rolePermissions.json";
    
    #endregion

    #endregion

    #endregion

    #region Constructors



    #endregion
    
    #region Methods
    // call after initialization
    private async Task ForceStorage()
    {
        Directory.CreateDirectory(DataFolder);
        
        await ForceFile(UserPath);
        await ForceFile(SubjectPath);
        await ForceFile(RoomPath);
        await ForceFile(RolePath);
        await ForceFile(PermissionPath);
        await ForceFile(LaboratoryPath);
        await ForceFile(GroupPath);
        
        await ForceFile(AssignmentPath);
        await ForceFile(AssessmentPath);
        
        await ForceFile(TeacherProfilePath);
        await ForceFile(StudentProfilePath);
        await ForceFile(PrincipalProfilePath);
        
        await ForceFile(TeacherSubjectPath);
        await ForceFile(SubjectEnrollmentPath);
        await ForceFile(RolePermissionPath);

        //await ForceFile(IdCounterPath);
    }

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

    #region Interacting with json files

    /// <summary>
    /// deserialize singular collection data from json file and set it to the list it belongs to
    /// </summary>
    public async Task<List<T>> Load<T>(string fileName)
    {
        var path = Path.Combine(DataFolder, fileName);
        var result = new List<T>();
        if (File.Exists(path))
        {
            var json = await File.ReadAllTextAsync(path);
            result = JsonSerializer.Deserialize<List<T>>(json) ?? [];
        }
        return result;
    }

    /// <summary>
    /// serialize singular collection set to json format and save to the file it belongs to
    /// </summary>
    public async Task Save<T>(string fileName, List<T> collection)
    {
        var path = Path.Combine(DataFolder, fileName);
        if (File.Exists(path))
        {
            var json = JsonSerializer.Serialize(collection);
            await File.WriteAllTextAsync(path, json);
        }
    }

    public async Task ForceFile(string path)
    {
        if (!File.Exists(path))
        {
            await File.AppendAllTextAsync(path, "[]");
        }
    }

    #endregion


    public async Task SeedData()
    {
        await SeedRoles();
        await SeedPermissions();
        await SeedSuperAdmin();
    }

    private async Task SeedRoles()
    {
        int maxId = GetMaxId(Roles);
        foreach(string roleName in Enum.GetNames<SchoolEnums.RoleName>())
        {
            Roles.Add(new(maxId += 1, roleName));
        }
    }
    private async Task SeedPermissions()
    {
        int maxId = GetMaxId(Permissions);
        foreach(string permission in Enum.GetNames<SchoolEnums.Permission>())
        {
            Permissions.Add(new(maxId += 1, permission));
        }
    }
    private async Task SeedSubjects()
    {
        if(Subjects.Count == 0)
        {
            int maxId = GetMaxId(Subjects);
            foreach(string subjectName in Enum.GetNames<SchoolEnums.SubjectName>())
            {
                Subjects.Add(new(maxId += 1, subjectName));
            }
        }
    }
    private async Task SeedSuperAdmin()
    {
        if(Users.Count > 0)
        {
            bool adminExists = Users
                .Any(user => user.RoleId == Roles.FirstOrDefault(role => role.RoleName == nameof(SchoolEnums.RoleName.SuperAdmin)).Id);
            if (!adminExists)
            {
                int maxId = GetMaxId(Users);
                var roleId = Roles.FirstOrDefault(role => role.RoleName == nameof(SchoolEnums.RoleName.SuperAdmin))?.Id;
                Users.Add(new(
                    maxId += 1,
                    "superadmin",
                    "superadmin",
                    new DateTime(2009, 04, 17),
                    "01255339127",
                    "superadmin@gmail.com",
                    "123",
                    (int)roleId));
            }
        }
    }


    public int GetMaxId<T>(List<T> collection) where T : BaseModel
    {
        int maxId = 0;
        if(collection is not null)
        {
            maxId = collection.Count > 0 ? collection.Select(item => item.Id).Max() : 0;
        }
        return maxId;
    }

    #endregion
}
