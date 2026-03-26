using SchoolManagementSystem.Data.Attributes;
using SchoolManagementSystem.Data.HelperClasses;
using SchoolManagementSystem.Data.Models.Base;

namespace SchoolManagementSystem.Data.Models;

[FileNamePrefix("rooms")]
public class Room : NamedModel
{
    [Reference<RoomType>]
    public int RoomTypeId { get; set; }
}