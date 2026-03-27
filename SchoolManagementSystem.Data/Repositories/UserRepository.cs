using ProjectHelperLibrary.Response;
using SchoolManagementSystem.Data.Config;
using SchoolManagementSystem.Data.HelperClasses;
using SchoolManagementSystem.Data.Models;
using SchoolManagementSystem.Data.Models.JoinedModels;
using SchoolManagementSystem.Data.Repositories.Base;

namespace SchoolManagementSystem.Data.Repositories;

public class UserRepository : BaseRepository<User>
{
    public UserRepository() : base(SchoolContext.Users)
    {
        
    }

    #region Collection
    public async Task<DataResponse<List<User>>> GetStudentsBySubjectEnrollments(List<SubjectEnrollment> subjectEnrollments)
    {
        var studentIds = subjectEnrollments.Select(se => se.StudentId).DistinctBy(se => se).ToList();
        var response = await GetWhere(user => 
            studentIds.Contains(user.Id)
            && user.RoleId == (int)SchoolEnums.RoleName.Student, "");
        
        return response;
    }
    
    public async Task<DataResponse<List<User>>> GetByRoleId(int roleId)
    {
        string roleName = Enum.GetName(typeof(SchoolEnums.RoleName), roleId) ?? roleId.ToString();
        var response = await GetWhere(user => user.RoleId == roleId, $"Could not find any {roleName}s");
        return response;
    }
    
    public async Task<DataResponse<List<User>>> GetByGroupId(int groupId)
    {
        var response = await GetWhere(user => user.GroupId == groupId, "No students found in the group");
        return response;
    }
    #endregion

    #region Single
    public async Task<DataResponse<User>> GetByFullName(string fullName)
    {
        var response = await GetSingle(
            user => user.FullName.Equals(fullName, StringComparison.OrdinalIgnoreCase),
            "Could not find user with name: " + fullName);
        return response;
    }

    public async Task<DataResponse<User>> GetByEmail(string email)
    {
        var response = await GetSingle(
            user => user.Email.Equals(email),
            $"Could not find user with email: {email}");
        return response;
    }

    public async Task<DataResponse<User>> GetByPrivateId(string privateId)
    {
        var response = await GetSingle(
            user => user.PrivateId.Equals(privateId, StringComparison.OrdinalIgnoreCase),
            $"User with privateId: {privateId} not found");
        return response;
    }

    public async Task<DataResponse<User>> GetByOfficeRoomId(int officeRoomId)
    {
        var response = await GetSingle(
            user => 
                user.OfficeRoomId == officeRoomId &&
                user.HasRole(SchoolEnums.RoleName.Principal),
            "Could not find user with that office room");
        return response;
    }

    public async Task<DataResponse<User>> GetTeacherByGroupId(int groupId)
    {
        var response = await GetSingle(
            user =>
                user.GroupId == groupId &&
                user.HasRole(SchoolEnums.RoleName.Teacher),
            "Could not find the teacher with this group");
        return response;
    }
    
    #endregion
    
    #region Exists by

    public async Task<bool> ExistsByEmail(string email)
    {
        return await ExistsAsync(user => user.Email.Equals(email));
    }

    public async Task<bool> ExistsByPrivateId(string privateId)
    {
        return await ExistsAsync(user => user.PrivateId.Equals(privateId));
    }
    
    #endregion

}