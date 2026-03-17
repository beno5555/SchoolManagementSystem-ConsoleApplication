namespace SchoolManagementSystem.Data.Models.UserProfiles;

public class TeacherProfile : User
{
    // group that is assigned to the teacher. (damrigebeli)
    public int? GroupId { get; set; }  
    public TeacherProfile(
        string firstName, string lastName, DateTime dateOfBirth, string privateId, string email, string passwordHash, 
        int? groupId) : base(firstName, lastName, dateOfBirth, privateId, email, passwordHash, 3)
    {
        if (groupId.HasValue)
        {
            GroupId = groupId.Value;
        }
    }
    // for principal
    public TeacherProfile(
        string firstName, string lastName, DateTime dateOfBirth, string privateId, string email, string passwordHash, int roleId, 
        int? groupId) : base(firstName, lastName, dateOfBirth, privateId, email, passwordHash, roleId)

    {
        if (groupId.HasValue)
        {
            GroupId = groupId.Value;
        }
    }
    
}