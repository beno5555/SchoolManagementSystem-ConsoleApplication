using System.Xml.XPath;
using SchoolManagementSystem.ConsoleDisplay;
using SchoolManagementSystem.Data.Config;
using SchoolManagementSystem.Data.Models.JoinedModels;
using SchoolManagementSystem.DTOCreation;
using SchoolManagementSystem.Service.BusinessLogic.Factories;
using SchoolManagementSystem.Service.DTOs.Groups;
using SchoolManagementSystem.Service.DTOs.User.Auth;
using Spectre.Console;

namespace SchoolManagementSystem.Controllers;

public class SuperAdminController
{
    private readonly ServiceFactory _services;
    private SessionUser?  _sessionUser;

    public SuperAdminController(ServiceFactory services)
    {
        _services = services;
    }

    public void SetUser(SessionUser sessionUser)
    {
        _sessionUser =  sessionUser;
    }

    public void ClearSession()
    {
        _sessionUser = null;
    }

    #region Register User

    public async Task RegisterUser()
    {
        LayoutHelper.RenderSectionTitle("Register a new user");
        var roleId = await PromptRoleId();

        if (roleId != -1)
        {
            var registerDTO = UserDTOCreation.PromptAdminRegistration(roleId);
            var registerResponse = await _services.AuthService.RegisterUser(registerDTO);
            if (registerResponse.Success)
            {
                LayoutHelper.ShowSuccess("Successfully Registered a new User");
            }
            else
            {
                LayoutHelper.ShowError("Registration Failed --> " +  registerResponse.Message);
            }
        }
    }

    private async Task<int> PromptRoleId()
    {
        int result = -1;
        var rolesResponse = await _services.UserService.GetRoles();

        if (rolesResponse.Success)
        {
            result = LayoutHelper.GetMenuChoice(rolesResponse.Value);
        }

        return result;
    }
    
    #endregion
    
    #region Add a new subject

    public async Task AddSubject(int adminId)
    {
        LayoutHelper.ShowInfo("Service is still in development..");
    }  
    
    #endregion
    
    #region Assign Subjects to Teachers

    public async Task AssignSubjectToTeacher()
    {
        LayoutHelper.RenderSectionTitle("Assign Subjects to Teachers");

        var (subjectId, teacherId) = await PromptAssignmentData();
        if (subjectId != -1 && teacherId != -1)
        {
            var assignResponse = await _services.StudentService.AssignSubjectToTeacher(subjectId, teacherId);

            if (assignResponse.Success)
            {
                LayoutHelper.ShowSuccess("Successfully Assigned a new Subject to a teacher");
            }
            else
            {
                LayoutHelper.ShowError("Assignment Failed --> " + assignResponse.Message);
            }
        }
    }

    private async Task<(int subjectId, int teacherId)> PromptAssignmentData()
    {
        int teacherId = await PromptTeacher();
        int subjectId = -1;
        if (teacherId != -1)
        {
            subjectId = await PromptSubject(teacherId);
        }
        return (subjectId, teacherId);
    }

    private async Task<int> PromptTeacher()
    {
        int result = -1;
        var teachersResponse = await _services.UserService.GetByRole(nameof(SchoolEnums.RoleName.Teacher));

        if (teachersResponse.Success)
        {
            result = LayoutHelper.GetMenuChoice(teachersResponse.Value);
        }
        else
        {
            LayoutHelper.ShowError("Could not retrieve teachers" + teachersResponse.Message);
        }

        return result;
    }

    private async Task<int> PromptSubject(int teacherId)
    {
        int result = -1;
        LayoutHelper.RenderSectionTitle("Pick a subject");
        var subjectsResponse = await _services.StudentService.GetSubjectsNotAssignedToTeacher(teacherId);

        if (subjectsResponse.Success)
        {
            result = LayoutHelper.GetMenuChoice(subjectsResponse.Value);
        }
        else
        {
            LayoutHelper.ShowError("Could not retrieve subjects not already associated with the teacher --> " + subjectsResponse.Message);
        }

        return result;
    }

    #endregion
    
    #region Manage Groups

    public async Task ManageGroups()
    {
        LayoutHelper.RenderSectionTitle("Groups");

        var options = new List<string>()
        {
            "Add a new group",
            "Assign class to a group",
            "Assign Student to a group"
        };
        var choice = LayoutHelper.GetMenuChoice(options);

        switch (choice)
        {
            case 1:
                await AddNewGroup();
                break;
            case 2:
                await AssignClassToGroup();
                break;
            case 3:
                await AssignStudentToGroup();
                break;
        }
    }

    private async Task AssignStudentToGroup()
    {
        LayoutHelper.RenderSectionTitle("Assign Student to Group");

    }

    private async Task AddNewGroup()
    {
        LayoutHelper.RenderSectionTitle("Add a new group");
        var dto = await PromptGroupDTO();

        if (dto is not null)
        {
            await _services.RoomService.CreateGroup(dto);
            LayoutHelper.ShowSuccess("Group added successfully");
        }
    }

    private async Task<CreateGroupDTO?> PromptGroupDTO()
    {
        CreateGroupDTO? result = null;
        var (teacherId, classRoomId) = await PromptGroupData();
        if (teacherId != -1 && classRoomId != -1)
        {
            result = new CreateGroupDTO
            {
                GroupName = LayoutHelper.GetInput("Group Name"),
                TeacherId = teacherId,
                ClassRoomId = classRoomId
            };
        }
        else
        {
            LayoutHelper.ShowError("Teacher or classroom ids were incorrect");
        }

        return result;
    }

    private async Task<(int teacherId, int classRoomId)> PromptGroupData()
    {
        var teacherId = await PromptTeacher();
        int classRoomId = -1;
        if (teacherId != -1)
        {
            classRoomId = await PromptClassRoom();
        }
        return (classRoomId, classRoomId);
    }

    private async Task<int> PromptClassRoom()
    {
        int result = -1;
        var classRoomsResponse = await _services.RoomService.GetClassRooms();
        if (classRoomsResponse.Success)
        {
            result = LayoutHelper.GetMenuChoice(classRoomsResponse.Value);
        }
        else
        {
            LayoutHelper.ShowError("Could not find any rooms eligible for assigning a group to");
        }

        return result;
    }



    public async Task AssignClassToGroup()
    {
        var groupClass = await PromptGroupClassDTO();
        if (groupClass is not null)
        {
            await _services.RoomService.CreateGroupClass(groupClass);
            LayoutHelper.ShowSuccess("Group assigned successfully");
        }
    }

    private async Task<CreateGroupClassDTO?> PromptGroupClassDTO()
    {
        CreateGroupClassDTO? result = null;
        var classesResponse = await _services.RoomService.GetClasses();
        if (classesResponse.Success)
        {
            var teacherSubjectName = await _services.StudentService.GetTeacherSubjectNames(classesResponse.Value);
            var groupsResponse = await _services.RoomService.GetGroups();
            if (groupsResponse.Success)
            {
                result = new CreateGroupClassDTO
                {
                    GroupId = LayoutHelper.GetMenuChoice(groupsResponse.Value),
                    ClassId = LayoutHelper.GetMenuChoice(classesResponse.Value),
                };
            }
            else
            {
                LayoutHelper.ShowError("There are currently no groups in the database");
            }
        }
        else
        {
            LayoutHelper.ShowError("There are currently no classes in the database");
        }

        return result;
    }


    #endregion

    #region Manage Rooms
    
    public async Task ManageRooms()
    {
        LayoutHelper.RenderSectionTitle("Rooms");

        var options = new List<string>
        {
            "Add a new room"
        };
        var choice = LayoutHelper.GetMenuChoice(options);
        switch (choice)
        {
            case 1:
                await AddNewRoom();
                break;
        }
    }

    private async Task AddNewRoom()
    {
        LayoutHelper.ShowInfo("Under development...");

        LayoutHelper.RenderSectionTitle("Add a new room");
        var roomDTO = await PromptRoomDTO();
        if (roomDTO is not null)
        {
            await _services.RoomService.CreateRoom(roomDTO);
            LayoutHelper.ShowSuccess("Room added successfully!");
        }
    }

    private async Task<CreateRoomDTO?> PromptRoomDTO()
    {
        CreateRoomDTO? result = null;
        var roomTypeId = await PromptRoomType();
        if (roomTypeId != -1)
        {
            result = new CreateRoomDTO
            {
                RoomName = LayoutHelper.GetInput("Room Name"),
                RoomTypeId = roomTypeId,
            };
        }

        return result;
    }

    private async Task<int> PromptRoomType()
    {
        int result = -1;
        var roomTypesResponse = await _services.RoomService.GetRoomTypes();

        if (roomTypesResponse.Success)
        {
            result = LayoutHelper.GetMenuChoice(roomTypesResponse.Value);
        }

        return result;
    }
    
    #endregion
}