using ProjectHelperLibrary.Utilities;
using SchoolManagementSystem.Data.Constants;

namespace SchoolManagementSystem.Data.Models;

public class User
{
    // temporary solution. will need to store the maximum id in a file later.
    private static int _idIncrement = 1;

    public int UserId { get; set; } = _idIncrement++;
    public string FirstName { get; private set; }
    public string LastName { get; private set; }
    public string FullName => $"{FirstName.ToCapitalized().Value} {LastName.ToCapitalized().Value}";
    public DateTime DateOfBirth { get; set; }
    public string PrivateId { get; private set; }
    public string Email { get; set; } 
    public string PasswordHash { get; set; }
    public DateTime RegisterDate { get; set; } = DateTime.Now;

    public int RoleId { get; set; }
    
    public User(string firstName, string lastName, DateTime dateOfBirth, string privateId, string email, string passwordHash, int roleId)
    {
        FirstName = firstName;
        LastName = lastName;
        DateOfBirth = dateOfBirth;
        PrivateId = privateId;
        Email = email;
        PasswordHash = passwordHash;
        RoleId = roleId;
    }
}