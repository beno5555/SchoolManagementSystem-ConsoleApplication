using SchoolManagementSystem.Data.Models.Named;
using SchoolManagementSystem.Data.Repositories.Base;

namespace SchoolManagementSystem.Data.Repositories.Named;

public class RoomTypeRepository : NamedModelRepository<RoomType>
{
    public RoomTypeRepository() : base(SchoolContext.RoomTypes)
    {
    }
    
}