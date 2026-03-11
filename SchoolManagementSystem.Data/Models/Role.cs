namespace SchoolManagementSystem.Data.Models;

public class Role
{
    // temporary solution
    private static int _idIncrement = 1;
    public int RoleId { get; set; } = _idIncrement++;
    public Constants.Enums.Role RoleName { get; set; } = Constants.Enums.Role.Guest;
    public List<Permission> Permissions { get; set; } = [];

}