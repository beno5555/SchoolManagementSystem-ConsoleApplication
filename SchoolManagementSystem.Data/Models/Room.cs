namespace SchoolManagementSystem.Data.Models;

public class Room : BaseModel
{
    public string RoomName { get; set; } 

    public Room(string roomName) : base()
    {
        RoomName = roomName;
    }
}