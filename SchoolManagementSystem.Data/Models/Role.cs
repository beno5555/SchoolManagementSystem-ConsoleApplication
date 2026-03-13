using SchoolManagementSystem.Data.Constants;

namespace SchoolManagementSystem.Data.Models;

public class Role
{
    // temporary solution
    private static int _idIncrement = 1;
    public int RoleId { get; set; } = _idIncrement++;
    public Enums.RoleName RoleName { get; set; } 

    public Role(Enums.RoleName roleName)
    {
        RoleName = roleName;
    }
}