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
            ("My Subjects",     "student_subjects"),
            ("My Grades",       "student_grades"),
            ("My Assignments",  "student_assignments"),
            ("My Teachers",     "student_teachers"),
            ("Logout",          "logout")
        },
        ["Teacher"] = new()
        {
            ("My Classes",      "teacher_classes"),
            ("My Students",     "teacher_students"),
            ("Enter Grades",    "teacher_grades"),
            ("Assignments",     "teacher_assignments"),
            ("Logout",          "logout")
        },
        ["Principal"] = new()
        {
            ("Manage Teachers", "principal_teachers"),
            ("Manage Students", "principal_students"),
            ("Manage Subjects", "principal_subjects"),
            ("Manage Rooms",    "principal_rooms"),
            ("View Reports",    "principal_reports"),
            ("Logout",          "logout")
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