
using SchoolManagementSystem.Data.Constants;

namespace SchoolManagementSystem.Data.Models.UserProfiles;

public class PrincipalProfile : User
{
    public PrincipalProfile(string firstName, string lastName, DateTime dateOfBirth, string privateId, string email, string passwordHash) : base(firstName, lastName, dateOfBirth, privateId, email, passwordHash, Enums.RoleName.Teacher)
    {
        
    }
}