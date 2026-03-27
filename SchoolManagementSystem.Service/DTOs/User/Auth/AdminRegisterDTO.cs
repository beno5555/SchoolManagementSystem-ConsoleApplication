namespace SchoolManagementSystem.Service.DTOs.User.Auth;

/// <summary>
/// register from administration
/// </summary>
public class AdminRegisterDTO : BaseRegisterDTO
{
    public int RoleId { get; set; }
    public int? GroupId { get; set; }
    public int? OfficeRoomId { get; set; }
}