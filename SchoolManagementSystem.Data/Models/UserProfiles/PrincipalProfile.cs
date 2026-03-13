
using SchoolManagementSystem.Data.Constants;

namespace SchoolManagementSystem.Data.Models.UserProfiles;

public class PrincipalProfile : TeacherProfile
{
    public int OfficeRoomId { get; set; }
    
    public PrincipalProfile(
        string firstName, string lastName, DateTime dateOfBirth, string privateId, string email, string passwordHash, 
        int? groupId, 
        int? officeRoomId) : base(firstName, lastName, dateOfBirth, privateId, email, passwordHash, 
        2 /* for principal */, 
        groupId)
    {
        if (officeRoomId.HasValue)
        {
            OfficeRoomId = officeRoomId.Value;
        }
    }
}