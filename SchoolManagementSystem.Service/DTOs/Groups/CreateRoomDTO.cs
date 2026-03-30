namespace SchoolManagementSystem.Service.DTOs.Groups;

public class CreateRoomDTO
{
    public string RoomName { get; set; } = string.Empty;
    public int RoomTypeId { get; set; }
}