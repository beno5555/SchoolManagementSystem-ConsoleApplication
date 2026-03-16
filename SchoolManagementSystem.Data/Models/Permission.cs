namespace SchoolManagementSystem.Data.Models;

public class Permission : BaseModel
{
    public string PermissionName { get; set; } 
    public Permission(int id, string permissionName) : base(id)
    {
        PermissionName = permissionName;
    }
}