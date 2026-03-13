namespace SchoolManagementSystem.Data.Models;

public class Room
{
    private static int _idIncrement = 1;
    public int RoomId { get; set; } = _idIncrement++;
    public string RoomName { get; set; } = string.Empty;
}