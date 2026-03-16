namespace SchoolManagementSystem.Data.Models;

public class Room : BaseModel
{
    public string RoomName { get; set; } 

    public Room(int id, string roomName) : base(id)
    {
        RoomName = roomName;
    }
}