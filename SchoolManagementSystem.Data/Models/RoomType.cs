using SchoolManagementSystem.Data.HelperClasses;
using SchoolManagementSystem.Data.Models.Base;

namespace SchoolManagementSystem.Data.Models;

[FileNamePrefix("roomTypes")]
public class RoomType : BaseModel
{
    public string RoomTypeName { get; set; }
    public RoomType(string roomTypeName)
    {
        RoomTypeName = roomTypeName;
    }
}
