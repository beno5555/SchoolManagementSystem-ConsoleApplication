using ProjectHelperLibrary.Response;
using SchoolManagementSystem.Data.Config;
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
}