using SchoolManagementSystem.ConsoleDisplay;
using SchoolManagementSystem.Service.BusinessLogic.Factories;
using SchoolManagementSystem.Service.DTOs.Academic.Assignments;
using SchoolManagementSystem.Service.DTOs.User.Auth;

namespace SchoolManagementSystem.Controllers;

public class TeacherController
{
    private SessionUser? _sessionUser;
    private ServiceFactory _services;

    public TeacherController(ServiceFactory services)
    {
        _services = services;
    }

    public void SetUser(SessionUser sessionUser)
    {
        _sessionUser = sessionUser;
    }

    public void ClearSession()
    {
        _sessionUser = null;
    }
    
    #region Upload Assignment

    public async Task UploadAssignment(int? teacherId = null)
    {
        int targetTeacherId =  teacherId ?? _sessionUser.Id;
        LayoutHelper.RenderSectionTitle("Upload Assignment");
        
        var (subjectId, groupId, assignmentTypeId)  = await PromptAssignmentValues(targetTeacherId);
        if (subjectId != -1 && groupId != -1 && assignmentTypeId != -1)
        {
            var assignmentDTO = PromptAssignment(subjectId, groupId, assignmentTypeId);
            
            var uploadResponse = await _services.StudentService.UploadAssignment(assignmentDTO, targetTeacherId);
            
            if (uploadResponse.Success)
            {
                LayoutHelper.ShowSuccess("Assignment uploaded successfully");
            }
            else
            {
                LayoutHelper.ShowError("Could not upload assignment --> " + uploadResponse.Message);
            }
        }
    }

    private async Task<(int subjetId, int groupId, int assignmentTypeId)> PromptAssignmentValues(
        int targetTeacherId)
    {
        var subjectId = await PickSubject(targetTeacherId);
        int groupId = -1;
        if (subjectId != -1)
        {
            groupId = await PickGroup(subjectId, targetTeacherId);
        }
        var assignmentTypeId = await PickAssignmentType();
        return (subjectId, groupId, assignmentTypeId);
    }

    private AssignmentUploadDTO PromptAssignment(int subjectId, int groupId, int assignmentTypeId)
    {
        return new AssignmentUploadDTO
        {
            SubjectId = subjectId,
            GroupId = groupId,
            AssignmentTypeId = assignmentTypeId,
            AssignmentName = LayoutHelper.GetInput("Assignment Name"),
            Description = LayoutHelper.GetInput("Description"),
            DueDate = LayoutHelper.GetDateInput("Due Date")
        };
    }

    private async Task<int> PickAssignmentType()
    {
        int result = -1;
        var assignmentTypesResponse = await _services.StudentService.GetAssignmentTypes();
        if (assignmentTypesResponse.Success)
        {
            result = LayoutHelper.GetMenuChoice(assignmentTypesResponse.Value);
        }
        else
        {
            LayoutHelper.ShowError("No Assignment types found --> " +  assignmentTypesResponse.Message);
        }

        return result;
    }

    private async Task<int> PickGroup(int subjectId, int teacherId)
    {
        int result = -1;
        var groupsResponse = await _services.StudentService.GetGroupsBySubjectAndTeacher(subjectId, teacherId);
        if (groupsResponse.Success)
        {
            result = LayoutHelper.GetMenuChoice(groupsResponse.Value);
        }
        else
        {
            LayoutHelper.ShowError("No groups found --> " + groupsResponse.Message);
        }
        
        return result;
    }

    private async Task<int> PickSubject(int teacherId)
    {
        int result = -1;
        var subjectsResponse = await _services.StudentService.GetSubjectsByTeacher(teacherId);
        
        if (subjectsResponse.Success)
        {
            result = LayoutHelper.GetMenuChoice(subjectsResponse.Value);
        }
        else
        {
            LayoutHelper.ShowError("No Subjects found --> " + subjectsResponse.Message);
        }

        return result;
    }
    
    #endregion
    
    #region Write Assessment

    public async Task WriteAssessment(int? teacherId = null)
    {
        LayoutHelper.ShowInfo("Service still in progress..");
    }
    
    #endregion
}