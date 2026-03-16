using System.Text.Json;
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
        
        
    }

    public async Task LoadData()
    {
        Users = await Load<User>(UserPath);
        Subjects = await Load<Subject>(SubjectPath);
        Rooms = await Load<Room>(RoomPath);
        Roles = await Load<Role>(RolePath);
        Permissions = await Load<Permission>(PermissionPath);
        Laboratories = await Load<Laboratory>(LaboratoryPath);
        Groups = await Load<Group>(GroupPath);
        
        Assignments = await Load<Assignment>(AssignmentPath);
        Assessments = await Load<Assessment>(AssessmentPath);
        
        TeacherProfiles = await Load<TeacherProfile>(TeacherProfilePath);
        StudentProfiles = await Load<StudentProfile>(StudentProfilePath);
        PrincipalProfiles = await Load<PrincipalProfile>(PrincipalProfilePath);
        
        TeacherSubjects = await Load<TeacherSubject>(TeacherSubjectPath);
        SubjectEnrollments = await Load<SubjectEnrollment>(SubjectEnrollmentPath);
        RolePermissions = await Load<RolePermission>(RolePermissionPath);
        
    }

    public void SaveData()
    {
        
    }

    #region Singles
    
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
    public async Task Save<T>(string fileName)
    {
        var path = Path.Combine(DataFolder, fileName);
        if (File.Exists(path))
        {
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

    #endregion
}