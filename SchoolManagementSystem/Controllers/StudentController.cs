using SchoolManagementSystem.ConsoleDisplay;
using SchoolManagementSystem.Service.BusinessLogic.Factories;
using SchoolManagementSystem.Service.DTOs.User.Auth;

namespace SchoolManagementSystem.Controllers;

public class StudentController
{
    private SessionUser? _user;
    private readonly ServiceFactory _services;
    public StudentController(ServiceFactory services)
    {
        _services = services;
    }

    public void SetUser(SessionUser sessionUser)
    {
        _user = sessionUser;
    }

    public void ClearSession()
    {
        _user = null;
    }
    public async Task ViewMyTeachers()
    {
        LayoutHelper.ShowInfo("Service is currently unavailable");
    }
    public async Task ViewAssignments()
    {
        LayoutHelper.ShowInfo("Service is currently unavailable");
    }
    public async Task ViewMySubjects()
    {
        LayoutHelper.ShowInfo("Service is currently unavailable");
    }

    public async Task ViewMyGrades(int? studentId = null)
    {
        int targetStudentId = studentId ?? _user.Id;
        LayoutHelper.RenderSectionTitle("My Grades");
        
        var options = new List<string>
        {
            "Grade in a specific subject",
            "My Overall grade"
        };
        
        LayoutHelper.RenderMenuOptions(options);
        var choice = LayoutHelper.GetMenuChoice(options);

        switch (choice)
        {
            case 1:
                await ViewSubjectGrade(targetStudentId);
                break;
            case 2:
                await ViewStudentGrade(targetStudentId);
                break;
        }
    }

    private async Task ViewSubjectGrade(int studentId)
    {
        var subjectId = await PickSubject();

        if (subjectId != -1)
        {
            var averageResponse = await _services.StudentService.GetAverageSubjectGrade(studentId, subjectId);
            var finalResponse = await _services.StudentService.GetFinalSubjectGrade(studentId, subjectId);

            if (finalResponse.Success && averageResponse.Success)
            {
                LayoutHelper.RenderGradeTable("Subject Grade", averageResponse.Value, finalResponse.Value);
            }
            else
            {
                LayoutHelper.ShowError("Could not retrieve subjects --> " + finalResponse.Message + " --> " + averageResponse.Message);
            }
        }
    }

    private async Task<int> PickSubject()
    {
        var result = -1;
        var subjectsResponse = await _services.StudentService.GetSubjectsByStudent(_user.Id);
        if (subjectsResponse.Success)
        {
            LayoutHelper.RenderSectionTitle("Select a Subject");
            result = LayoutHelper.GetMenuChoice(subjectsResponse.Value);
        }
        else
        {
            LayoutHelper.ShowError("Could not retrieve subjects --> " + subjectsResponse.Message);
        }
        return result;
    }

    private async Task ViewStudentGrade(int studentId)
    {
        var studentAverageGradeResponse = await _services.StudentService.GetAverageStudentGrade(studentId);
        var studentFinalGradeResponse = await _services.StudentService.GetFinalStudentGrade(studentId);
        
        var allAveragesResponse = await _services.StudentService.GetAllAverageSubjectGrades(studentId);
        var allFinalsResponse = await _services.StudentService.GetAllFinalSubjectGrades(studentId);
        
        var subjectsResponse = await _services.StudentService.GetSubjectsByStudent(studentId);
        
        bool successfulDataRetrieval = studentAverageGradeResponse.Success && 
                                       studentFinalGradeResponse.Success &&
                                       allAveragesResponse.Success &&
                                       allFinalsResponse.Success &&
                                       subjectsResponse.Success;
        if (successfulDataRetrieval)
        {
            LayoutHelper.RenderStudentGradeTable(
                subjectsResponse.Value,
                allAveragesResponse.Value,
                allFinalsResponse.Value,
                studentAverageGradeResponse.Value,
                studentFinalGradeResponse.Value);
        }
        else
        {
            string first = !studentAverageGradeResponse.Success ? $" --> {studentAverageGradeResponse.Message}" : string.Empty;
            string second = !studentFinalGradeResponse.Success ? $" --> {studentFinalGradeResponse.Message}" : string.Empty;
            string third = !allAveragesResponse.Success ? $" --> {allAveragesResponse.Message}" : string.Empty;
            string fourth = !allFinalsResponse.Success ? $" --> {allFinalsResponse.Message}" : string.Empty;
            string fifth = !subjectsResponse.Success ? $" --> {subjectsResponse.Message}" : string.Empty;
            LayoutHelper.ShowError($"Could not retrieve data --> {first}{second}{third}{fourth}{fifth}");
        }
    }
}