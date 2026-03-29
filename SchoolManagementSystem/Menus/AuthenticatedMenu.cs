using SchoolManagementSystem.ConsoleDisplay;

namespace SchoolManagementSystem.Menus;

public static class AuthenticatedMenu
{
    // Each role maps to a list of (display label, action key) pairs.
    // The last entry is always Logout.
    private static readonly Dictionary<string, List<(string Label, string Action)>> RoleMenus = new()
    {
        ["Student"] = new()
        {
            ("My Subjects",     nameof(MenuConstants.ActionEnums.StudentAction.Subjects)),
            ("My Grades",       nameof(MenuConstants.ActionEnums.StudentAction.ViewMyGrades)),
            ("My Assignments",  nameof(MenuConstants.ActionEnums.StudentAction.Assignments)),
            ("My Teachers",     nameof(MenuConstants.ActionEnums.StudentAction.Teachers)),
            ("Logout",          nameof(MenuConstants.ActionEnums.SharedAction.Logout))
        },
        ["Teacher"] = new()
        {
            ("My Classes",    nameof(MenuConstants.ActionEnums.TeacherAction.Classes)),
            ("My Students",   nameof(MenuConstants.ActionEnums.TeacherAction.Students)),
            ("Enter Grades",  nameof(MenuConstants.ActionEnums.TeacherAction.EnterGrades)),
            ("Assignments",   nameof(MenuConstants.ActionEnums.TeacherAction.Assignments)),
            ("Logout",        nameof(MenuConstants.ActionEnums.SharedAction.Logout))
        },
        ["Principal"] = new()
        {
            ("Manage Teachers", nameof(MenuConstants.ActionEnums.PrincipalAction.Teachers)),
            ("Manage Students", nameof(MenuConstants.ActionEnums.PrincipalAction.Students)),
            ("Manage Subjects", nameof(MenuConstants.ActionEnums.PrincipalAction.Subjects)),
            ("Manage Rooms",    nameof(MenuConstants.ActionEnums.PrincipalAction.Rooms)),
            ("View Reports",    nameof(MenuConstants.ActionEnums.PrincipalAction.ViewGradeReports)),
            ("Logout",          nameof(MenuConstants.ActionEnums.SharedAction.Logout))
        },
        ["SuperAdmin"] = new()
        {
            ("Manage Users",        "admin_users"),
            ("Manage Roles",        "admin_roles"),
            ("Manage Permissions",  "admin_permissions"),
            ("Manage Subjects",     "admin_subjects"),
            ("Manage Rooms",        "admin_rooms"),
            ("Manage Groups",       "admin_groups"),
            ("Logout",              "logout")
        }
    };      

    /// <summary>
    /// Renders the role-specific menu and returns the selected action key.
    /// </summary>
    public static string Run(string fullName, string role)
    {
        LayoutHelper.RenderUserHeader(fullName, role);

        var options = RoleMenus.GetValueOrDefault(role);

        if (options == null || options.Count == 0)
        {
            LayoutHelper.ShowError($"No menu defined for role: {role}");
            return nameof(MenuConstants.ActionEnums.SharedAction.Logout);
        }

        LayoutHelper.RenderMenuOptions(options.Select(o => o.Label).ToList());

        var choice = LayoutHelper.GetMenuChoice(1, options.Count);
        return options[choice - 1].Action;
    }
}