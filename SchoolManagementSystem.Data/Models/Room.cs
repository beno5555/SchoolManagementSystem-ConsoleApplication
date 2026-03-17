using SchoolManagementSystem.Data.Config;
using SchoolManagementSystem.Data.Models.Base;

namespace SchoolManagementSystem.Data.Models;

public class Room : BaseModel
{
    public string RoomName { get; set; }
    public SchoolEnums.RoomType RoomType { get; set; }

    public Room(string roomName, SchoolEnums.RoomType roomType) : base()
    {
        RoomName = roomName;
        RoomType = roomType;
    }
}