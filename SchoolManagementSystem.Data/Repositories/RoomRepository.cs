using SchoolManagementSystem.Data.Models;
using SchoolManagementSystem.Data.Repositories.Base;

namespace SchoolManagementSystem.Data.Repositories;

public class RoomRepository : BaseRepository<Room>
{
    public RoomRepository() : base(SchoolContext.Rooms)
    {
    }
}