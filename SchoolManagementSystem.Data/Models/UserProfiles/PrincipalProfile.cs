
using SchoolManagementSystem.Data.Constants;

namespace SchoolManagementSystem.Data.Models.UserProfiles;

public class PrincipalProfile : User
{
    public PrincipalProfile(Person person, string email, string passwordHash) : base(person, email, passwordHash, Enums.RoleName.Principal)
    {
    }
}