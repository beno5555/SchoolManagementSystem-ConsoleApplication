using SchoolManagementSystem.Data.Constants;
using SchoolManagementSystem.Data.Models.UserProfiles;

namespace SchoolManagementSystem.Data.Models;

public class User
{
    // temporary solution. will need to store the maximum id in a file later.
    private static int _idIncrement = 1;

    public int UserId { get; set; } = _idIncrement++;
    public Person? Person { get; set; }
    public string Email { get; set; } 
    public string PasswordHash { get; set; }
    public DateTime RegisterDate { get; set; } = DateTime.Now;

    public Role Role { get; set; }
    
    public User(Person person, string email, string passwordHash, Enums.RoleName roleName)
    {
        Person = person;
        Email = email;
        PasswordHash = passwordHash;
        Role = new Role(roleName);
    }
}