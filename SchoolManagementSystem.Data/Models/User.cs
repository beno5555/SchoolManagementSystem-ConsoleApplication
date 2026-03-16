using ProjectHelperLibrary.Utilities;

namespace SchoolManagementSystem.Data.Models;

public class User : BaseModel
{

    public string FirstName { get; private set; }
    public string LastName { get; private set; }
    public string FullName => $"{FirstName.ToCapitalized().Value} {LastName.ToCapitalized().Value}";
    public DateTime DateOfBirth { get; set; }
    public string PrivateId { get; private set; }
    public string Email { get; set; } 
    public string PasswordHash { get; set; }
    public DateTime RegisterDate { get; set; } = DateTime.Now;

    public int RoleId { get; set; }
    
    public User(int id, string firstName, string lastName, DateTime dateOfBirth, string privateId, string email, string passwordHash, int roleId) : base(id)
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