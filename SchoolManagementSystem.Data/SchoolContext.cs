using SchoolManagementSystem.Data.Models;
using SchoolManagementSystem.Data.Models.JoinedModels;
using SchoolManagementSystem.Data.Models.UserProfiles;
using System;

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

    #region Methods

    public void LoadData()
    {
        
    }

    public void SaveData()
    {
        
    }

    /// <summary>
    /// deserialize singular collection data from json file and set it to the list it belongs to
    /// </summary>
    public void Load()
    {
        
    }

    /// <summary>
    /// serialize singular collection set to json format and save to the file it belongs to
    /// </summary>
    public void Save()
    {
        
    }

    #endregion
}
