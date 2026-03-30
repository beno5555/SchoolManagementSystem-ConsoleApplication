using ProjectHelperLibrary.Response;
using ProjectHelperLibrary.Utilities;
using SchoolManagementSystem.Data.Config;
using SchoolManagementSystem.Data.Models.JoinedModels;
using SchoolManagementSystem.Data.Models.Named;
using SchoolManagementSystem.Service.BusinessLogic.Factories;
using SchoolManagementSystem.Service.DTOs.Groups;

namespace SchoolManagementSystem.Service.BusinessLogic.Services;

public class RoomService
{
    private readonly UtilityFactory _utilities;
    public RoomService(UtilityFactory utilities)
    {
        _utilities = utilities;
    }

    public async Task<DataResponse<List<Room>>> GetClassRooms()
    {
        var response = new  DataResponse<List<Room>>();
        var typesResponse =
            await _utilities.Repos.RoomTypeRepository.GetByName(nameof(SchoolEnums.RoomTypeName.Classroom));
        if (typesResponse.Success)
        {
            response = await _utilities.Repos.RoomRepository.GetByTypeId(typesResponse.Value.Id);
        }

        return response;
    }

    public async Task CreateGroup(CreateGroupDTO groupDTO)
    {
        var group = new Group
        {
            Name = groupDTO.GroupName,
            ClassroomId = groupDTO.ClassRoomId,
            TeacherId = groupDTO.TeacherId
        };
        await _utilities.Repos.GroupRepository.AddAsync(group);
    }

    public async Task<DataResponse<List<RoomType>>> GetRoomTypes()
    {
        return await _utilities.Repos.RoomTypeRepository.GetAll();
    }

    public async Task CreateRoom(CreateRoomDTO roomDTO)
    {
        var room = new Room
        {
            Name = roomDTO.RoomName,
            RoomTypeId = roomDTO.RoomTypeId,
        };
        await _utilities.Repos.RoomRepository.AddAsync(room);
    }

    public async Task<DataResponse<List<Room>>> GetRooms()
    {
        var response = new  DataResponse<List<Room>>();
        var classRoomTypeId = await _utilities.Repos.RoomTypeRepository.GetIdByName(nameof(SchoolEnums.RoomTypeName.Classroom));
        if (classRoomTypeId != -1)
        {
            response = await _utilities.Repos.RoomRepository.GetByTypeId(classRoomTypeId);
        }
        else
        {
            response.SetStatus(false, "classroom type does not exist in the database");
        }

        return response;
    }

    public async Task<DataResponse<List<Group>>> GetGroups()
    {
        return await _utilities.Repos.GroupRepository.GetAll();
    }

    public async Task<DataResponse<List<SchoolClass>>> GetClasses()
    {
        return await _utilities.Repos.SchoolClassRepository.GetAll();
    }

    public async Task CreateGroupClass(CreateGroupClassDTO groupClassDTO)
    {
        var groupClass = new GroupClass
        {
            GroupId = groupClassDTO.GroupId,
            SchoolClassId = groupClassDTO.ClassId
        };
        await _utilities.Repos.GroupClassRepository.AddAsync(groupClass);
    }
}