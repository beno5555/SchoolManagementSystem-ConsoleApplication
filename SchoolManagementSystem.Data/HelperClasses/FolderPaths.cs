namespace SchoolManagementSystem.Data.HelperClasses;

public static class FolderPaths
{
    public const string DataFolder = "DataFolder";
    //private const string IdCounterPath = "idCounters.json";
    
    #region Main
    
    public const string UserPath = "users.json";
    public const string SubjectPath = "subjects.json";
    public const string RoomPath = "rooms.json";
    public const string RolePath = "roles.json";
    public const string PermissionPath = "permissions.json";
    public const string LaboratoryPath = "laboratories.json";
    public const string GroupPath = "groups.json";
    
    #endregion
 
    #region Academic
    
    public const string AssignmentPath = "assignments.json";
    public const string AssessmentPath = "assessments.json";
    public const string AssignmentTypePath = "assignmentTypes.json";
    #endregion
    
    #region Profiles

    public const string TeacherProfilePath = "teacherProfiles.json";
    public const string StudentProfilePath = "studentProfiles.json";
    public const string PrincipalProfilePath = "principalProfiles.json";
    
    #endregion
    
    #region Joined

    public const string TeacherSubjectPath = "teacherSubjects.json";
    public const string SubjectEnrollmentPath = "subjectEnrollments.json";
    public const string RolePermissionPath = "rolePermissions.json";
    
    #endregion

}