using SchoolManagementSystem.Data.Attributes;
using SchoolManagementSystem.Data.HelperClasses;
using SchoolManagementSystem.Data.Models.Base;

namespace SchoolManagementSystem.Data.Models;

[FileNamePrefix("roomTypes")]
public class RoomType : NamedModel
{
    public RoomType(string roomTypeName) : base(roomTypeName)
    {
        
    }

    public RoomType()
    {
        
    }
}
