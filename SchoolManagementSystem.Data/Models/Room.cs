using SchoolManagementSystem.Data.Config;
using SchoolManagementSystem.Data.Models.Base;

namespace SchoolManagementSystem.Data.Models;

public class Room : BaseModel
{
    public string RoomName { get; set; }
    public int RoomTypeId { get; set; }
    public RoomType RoomType { get; set; }
    public Room(string roomName, int roomTypeId) : base()
    {
        RoomName = roomName;
        RoomTypeId = roomTypeId;
    }
}