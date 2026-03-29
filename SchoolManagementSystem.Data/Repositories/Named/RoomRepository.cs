using SchoolManagementSystem.Data.Models.Named;
using SchoolManagementSystem.Data.Repositories.Base;

namespace SchoolManagementSystem.Data.Repositories.Named;

public class RoomRepository : NamedModelRepository<Room>
{
    public RoomRepository() : base(SchoolContext.Rooms)
    {
    }
}