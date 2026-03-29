using System.Xml;

namespace SchoolManagementSystem.Menus;

public static class MenuConstants
{
    public static class ActionEnums
    {
        public enum SharedAction
        {
            Logout = 1
        }

        public enum StudentAction
        {
            Subjects = 1,
            ViewMyGrades,
            Assignments,
            Teachers
        }

        public enum TeacherAction
        {
            Classes = 1,
            Students,
            Assessment,
            Assignment,
            EnterGrades,
            Assignments
        }

        public enum PrincipalAction
        {
            Teachers = 1,
            Students,
            Subjects,
            Rooms,
            ViewGradeReports
        }

        public enum SuperAdminAction
        {
            Users = 1,
            Roles,
            Permissions,
            Subjects,
            Rooms,
            Groups
        }
    }

    public static class Roles
    {
        public const string Teacher = "Teacher";
        public const string Student = "Student";    
        public const string Principal = "Principal";    
        public const string SuperAdmin = "SuperAdmin";
    }

}