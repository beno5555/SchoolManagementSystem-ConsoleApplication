namespace SchoolManagementSystem.Data.HelperClasses;

public static class FolderPaths
{
    public const string DataFolder = "DataFolder";
    
    public static string DataPath => Path.Combine(
        AppContext.BaseDirectory, 
        @"..\..\..\..\SchoolManagementSystem.Data", 
        DataFolder);        
    
    public const string UserPath = "users.json";
    public const string SubjectPath = "subjects.json";
    public const string RoomPath = "rooms.json";
    public const string RolePath = "roles.json";
    public const string PermissionPath = "permissions.json";
    public const string LaboratoryPath = "laboratories.json";
    public const string GroupPath = "groups.json";
    
    
    public const string AssignmentPath = "assignments.json";
    public const string AssessmentPath = "assessments.json";
    public const string AssignmentTypePath = "assignmentTypes.json";
    

    public const string TeacherProfilePath = "teacherProfiles.json";
    public const string StudentProfilePath = "studentProfiles.json";
    public const string PrincipalProfilePath = "principalProfiles.json";
    
    

    public const string TeacherSubjectPath = "teacherSubjects.json";
    public const string SubjectEnrollmentPath = "subjectEnrollments.json";
    public const string RolePermissionPath = "rolePermissions.json";


    public static readonly string[] JsonPaths =
    [
        UserPath,
        SubjectPath,
        RoomPath,
        RolePath,
        PermissionPath,
        LaboratoryPath,
        GroupPath,
        
        AssignmentPath,
        AssessmentPath,
        AssignmentTypePath,
        
        TeacherProfilePath,
        StudentProfilePath,
        PrincipalProfilePath,
        
        TeacherSubjectPath,
        SubjectEnrollmentPath,
        RolePermissionPath
    ];

    public static string GetFullPath(string fileName)
    {
        return Path.Combine(DataPath, fileName);
    }
}