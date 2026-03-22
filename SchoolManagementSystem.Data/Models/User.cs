using ProjectHelperLibrary.Utilities;
using SchoolManagementSystem.Data.Attributes;
using SchoolManagementSystem.Data.Config;
using SchoolManagementSystem.Data.HelperClasses;
using SchoolManagementSystem.Data.Models.Base;

namespace SchoolManagementSystem.Data.Models;

[FileNamePrefix("users")]
public class User : BaseModel
{
    #region Properties
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string FullName => $"{FirstName.ToCapitalized()} {LastName.ToCapitalized()}";
    public DateTime DateOfBirth { get; set; }
    public string PrivateId { get; set; }
    
    public string Email { get; set; } 
    public string PasswordHash { get; set; }
    public DateTime RegisterDate { get; set; } = DateTime.Now;
    
    #region Role-Specific
    [Reference<Role>]
    public int RoleId { get; set; }
    
    [Reference<Group>]
    public int? GroupId { get; set; } // mandatory for students, optional for teachers
    
    public int? FinalGrade { get; set; } // student-specific
    
    [Reference<Room>]
    public int? OfficeRoomId { get; set; } // principal-specific
    
    #endregion
    
    
    #endregion
    
    #region Constructors
    
    public User(
        string firstName, 
        string lastName, 
        DateTime dateOfBirth, 
        string privateId, 
        string email, 
        string passwordHash, 
        int roleId,
        
        int? groupId = null,
        int? officeRoomId = null
        )
    {
        FirstName = firstName;
        LastName = lastName;
        DateOfBirth = dateOfBirth;
        PrivateId = privateId;
        Email = email;
        PasswordHash = passwordHash;
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
        DateTime dateOfBirth,
        string privateId,
        string email,
        string passwordHash,
        int groupId
    )
    {
        
        return new User(firstName, lastName, dateOfBirth, privateId, email, passwordHash,
            (int)SchoolEnums.RoleName.Student, groupId: groupId);
    }

    public static User CreateTeacher(
        string firstName,
        string lastName,
        DateTime dateOfBirth,
        string privateId,
        string email,
        string passwordHash,
        int? groupId)
    {
        return new User(firstName, lastName, dateOfBirth, privateId, email, passwordHash,
            (int)SchoolEnums.RoleName.Teacher, groupId: groupId);
    }

    public static User CreatePrincipal(
        string firstName,
        string lastName,
        DateTime dateOfBirth,
        string privateId,
        string email,
        string passwordHash,
        int officeRoomId,
        int? groupId)
    {
        return new User(firstName, lastName, dateOfBirth, privateId, email, passwordHash,
            (int)SchoolEnums.RoleName.Principal, groupId: groupId, officeRoomId: officeRoomId);
    }
    
    #endregion
}