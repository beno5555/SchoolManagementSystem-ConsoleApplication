using SchoolManagementSystem.Data.HelperClasses;
using SchoolManagementSystem.Data.Models.Base;

namespace SchoolManagementSystem.Data.Models;

[FileNamePrefix("rooms")]
public class Room : BaseModel
{
    public string RoomName { get; set; }
    public int RoomTypeId { get; set; }
    public Room(string roomName, int roomTypeId) 
    {
        RoomName = roomName;
        RoomTypeId = roomTypeId;
    }
}