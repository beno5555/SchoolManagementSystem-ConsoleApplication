using SchoolManagementSystem.Data.Models;

namespace SchoolManagementSystem.Data.Repositories;

public class UserRepository : BaseRepository<User>
{
    public UserRepository(SchoolContext context, List<User> collection) : base(context, collection)
    {
        
    }
    
}