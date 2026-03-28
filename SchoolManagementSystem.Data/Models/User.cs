using System.Text.Json.Serialization;
using ProjectHelperLibrary.Utilities;
using SchoolManagementSystem.Data.Attributes;
using SchoolManagementSystem.Data.Config;
using SchoolManagementSystem.Data.Models.Base;
using SchoolManagementSystem.Data.Models.Named;

namespace SchoolManagementSystem.Data.Models;

[FileNamePrefix("users")]
public class User : BaseModel
{
    #region Properties

    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    
    [JsonIgnore]
    public string FullName => $"{FirstName.ToCapitalized()} {LastName.ToCapitalized()}";
    public string PhoneNumber { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public string PrivateId { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public string PasswordSalt { get; set; } = string.Empty;
    public DateTime DateOfBirth { get; set; } 
    public DateTime RegistrationDate { get; set; } = DateTime.Now;
    
    #region Role-Specific

    [Reference<Role>] public int RoleId { get; set; } = (int)SchoolEnums.RoleName.Student;
    
    [Reference<Group>]
    public int? GroupId { get; set; } // mandatory for students, optional for teachers

    public decimal? AverageGrade { get; set; } // student-specific
    public int? FinalGrade { get; set; } // student-specific
    
    [Reference<Room>]
    public int? OfficeRoomId { get; set; } // principal-specific
    
    #endregion
    
    #endregion
    
    #region Constructors

    public User()
    {
        
    }
    public User(
        string firstName, 
        string lastName, 
        string phoneNumber,
        string address,
        DateTime dateOfBirth, 
        string privateId, 
        string email, 
        string passwordHash,
        string passwordSalt,
        int roleId,
        
        int? groupId = null,
        int? officeRoomId = null
        )
    {
        FirstName = firstName;
        LastName = lastName;
        PhoneNumber = phoneNumber;
        Address = address;
        DateOfBirth = dateOfBirth;
        PrivateId = privateId;
        Email = email;
        PasswordHash = passwordHash;
        PasswordSalt = passwordSalt;
        RoleId = roleId;
        GroupId = groupId;
        OfficeRoomId = officeRoomId;

        if (roleId.Equals((int)SchoolEnums.RoleName.Student))
        {
            FinalGrade = 0;
        }
    }

    public static User CreateStudent(
        string firstName,
        string lastName,
        string phoneNumber,
        string address,
        DateTime dateOfBirth,
        string privateId,
        string email,
        string passwordHash,
        string passwordSalt,
        int groupId
    )
    {
        
        return new User(firstName, lastName, phoneNumber, address, dateOfBirth, privateId, email, passwordHash, passwordSalt,
            (int)SchoolEnums.RoleName.Student, groupId: groupId);
    }

    public static User CreateTeacher(
        string firstName,
        string lastName,
        string phoneNumber, 
        string address,
        DateTime dateOfBirth,
        string privateId,
        string email,
        string passwordHash,
        string passwordSalt,
        int? groupId)
    {
        return new User(firstName, lastName, phoneNumber,address, dateOfBirth, privateId, email, passwordHash, passwordSalt,
            (int)SchoolEnums.RoleName.Teacher, groupId: groupId);
    }

    public static User CreatePrincipal(
        string firstName,
        string lastName,
        string phoneNumber,
        string address,
        DateTime dateOfBirth,
        string privateId,
        string email,
        string passwordHash,
        string passwordSalt,
        int officeRoomId,
        int? groupId)
    {
        return new User(firstName, lastName, phoneNumber, address, dateOfBirth, privateId, email, passwordHash, passwordSalt,
            (int)SchoolEnums.RoleName.Principal, groupId: groupId, officeRoomId: officeRoomId);
    }
    
    #endregion
}