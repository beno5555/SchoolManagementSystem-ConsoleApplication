using SchoolManagementSystem.Data.Models.Base;

namespace SchoolManagementSystem.Data.Models
{
    public class RoomType : BaseModel
    {
        public string RoomTypeName { get; set; }
        public RoomType(string roomTypeName)
        {
            RoomTypeName = roomTypeName;
        }
    }
}
