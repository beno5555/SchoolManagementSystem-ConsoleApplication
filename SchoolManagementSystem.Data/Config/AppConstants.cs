using System.Reflection;
using SchoolManagementSystem.Data.Attributes;
using SchoolManagementSystem.Data.HelperClasses;

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

        public static List<string> JsonPaths = [];
        
        #region Get Full Path including project directory
        
        public static string GetFullPath(string fileName)
        {
            var path = Path.Combine(DataPath, fileName);
            var directory = Path.GetDirectoryName(path) ?? string.Empty;
            Directory.CreateDirectory(directory);
            return path;
        }

        public static string GetFullPath(Type type)
        {
            var fileName = GetFileName(type);
            var path = GetFullPath(fileName);
            return path;
        }

        public static string GetFullPath<T>()
        {
            return GetFullPath(typeof(T));
        }
        
        #endregion
        
        #region Get File Name based on type
        
        public static string GetFileName<T>()
        {
            return GetFileName(typeof(T));
        }

        public static string GetFileName(Type type)
        {
            string fileNamePrefix = type.GetCustomAttribute<FileNamePrefixAttribute>()?.Name ?? $"{type.Name}s";
            string fileName = fileNamePrefix + ".json";
            return fileName;
        }
        
        #endregion
    }

    public static class Defaults
    {
        public const int GradeValue = 0;
        public const string SubjectName = "'unassigned'";
        // ... 
    }
}