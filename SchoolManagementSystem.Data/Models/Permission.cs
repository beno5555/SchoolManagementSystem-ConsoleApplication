namespace SchoolManagementSystem.Data.Models;

public class Permission
{
    public int PermissionId { get; set; }
    public required Constants.Enums.Permission PermissionName { get; set; }
    public List<Role> Roles { get; set; } = [];
}