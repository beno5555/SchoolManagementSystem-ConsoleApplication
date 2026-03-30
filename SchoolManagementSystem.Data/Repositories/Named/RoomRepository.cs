using ProjectHelperLibrary.Response;
using SchoolManagementSystem.Data.Models.Named;
using SchoolManagementSystem.Data.Repositories.Base;

namespace SchoolManagementSystem.Data.Repositories.Named;

public class RoomRepository : NamedModelRepository<Room>
{
    public RoomRepository() : base(SchoolContext.Rooms)
    {
    }

    public async Task<DataResponse<List<Room>>> GetByTypeId(int roomTypeId)
    {
        return await GetWhere(
            room => roomTypeId == room.RoomTypeId,
            "No rooms are registered with that room type");
    }
}