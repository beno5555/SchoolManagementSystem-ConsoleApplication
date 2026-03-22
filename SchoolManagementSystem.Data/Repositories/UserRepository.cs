using ProjectHelperLibrary.Response;
using SchoolManagementSystem.Data.Models;
using SchoolManagementSystem.Data.Repositories.Base;

namespace SchoolManagementSystem.Data.Repositories;

public class UserRepository : BaseRepository<User>
{
    public UserRepository(List<User> collection) : base(collection)
    {
        
    }

    
}