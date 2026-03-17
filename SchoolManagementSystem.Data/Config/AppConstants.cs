namespace SchoolManagementSystem.Data.Config;

public static class AppConstants
{
    public static class MaximumCount
    {
        private const int Teachers = 100;
        public const int Users = 1000;
        public const int Subjects = 50;
        public const int Rooms = 300;
        public const int Roles = 20;
        public const int Permissions = 50;
        public const int Groups = 300;
        
        public const int AssignmentTypes = 20;
        public const int Assignments = 100_000;
        public const int Assessments = 100_000;
        
        public const int TeacherSubjects = Teachers * Subjects;
        public const int SubjectEnrollments = Users * Subjects;
        public const int RolePermissions = Roles * Permissions;

    }
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
        public const string GroupPath = "groups.json";
    
    
        public const string AssignmentPath = "assignments.json";
        public const string AssessmentPath = "assessments.json";
        public const string AssignmentTypePath = "assignmentTypes.json";
    

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
            GroupPath,
        
            AssignmentPath,
            AssessmentPath,
            AssignmentTypePath,

            TeacherSubjectPath,
            SubjectEnrollmentPath,
            RolePermissionPath
        ];

        public static string GetFullPath(string fileName)
        {
            var path = Path.Combine(DataPath, fileName);
            var directory = Path.GetDirectoryName(path) ?? string.Empty;
            Directory.CreateDirectory(directory);
            return path;
        }
    }

    public static class Defaults
    {
        public const int GradeValue = 0;
        public const string SubjectName = "'unassigned'";
        // ... 
    }
}