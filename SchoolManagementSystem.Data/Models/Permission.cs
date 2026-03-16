namespace SchoolManagementSystem.Data.Models;

public class Permission : BaseModel
{
    public string PermissionName { get; set; } = string.Empty;
    public Permission(int id, string permissionName)
    {
        Id = id;
        PermissionName = permissionName;
    }
}