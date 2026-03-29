using SchoolManagementSystem.ConsoleDisplay;
using SchoolManagementSystem.Service.BusinessLogic.Factories;
using SchoolManagementSystem.Service.DTOs.User.Auth;

namespace SchoolManagementSystem.Controllers;

public class StudentController
{
    private SessionUser? _user;
    private readonly ServiceFactory _services;
    public StudentController(ServiceFactory services, SessionUser sessionUser)
    {
        _services = services;
    }

    public void SetUser(SessionUser? sessionUser)
    {
        _user = sessionUser;
    }

    public void ClearSession()
    {
        _user = null;
    }
    public async Task ViewMyTeachers()
    {
        throw new NotImplementedException();
    }
    public async Task ViewAssignments()
    {
        throw new NotImplementedException();
    }
    public async Task ViewMySubjects()
    {
        LayoutHelper.RenderSectionTitle("My Subjects");
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
        var choice = LayoutHelper.GetMenuChoice(1, options.Count);

        switch (choice)
        {
            case 1:
                await ViewSubjectGrade(targetStudentId);
                break;
            case 2:
                await ViewStudentGrade();
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
            var subjects = subjectsResponse.Value;
            LayoutHelper.RenderSectionTitle("Select a Subject");
            LayoutHelper.RenderMenuOptions(subjectsResponse.Value.Select(s => s.Name).ToList());
            
            var choice = LayoutHelper.GetMenuChoice(1, subjects.Count);
            result = subjects[choice - 1].Id;
        }
        else
        {
            LayoutHelper.ShowError("Could not retrieve subjects --> " + subjectsResponse.Message);
        }
        return result;
    }

    private async Task ViewStudentGrade()
    {
        var studentAverageGradeResponse = await _services.StudentService.GetAverageStudentGrade(_user.Id);
        var studentFinalGradeResponse = await _services.StudentService.GetFinalStudentGrade(_user.Id);
        
        var allAveragesResponse = await _services.StudentService.GetAllAverageSubjectGrades(_user.Id);
        var allFinalsResponse = await _services.StudentService.GetAllFinalSubjectGrades(_user.Id);
        
        var subjectsResponse = await _services.StudentService.GetSubjectsByStudent(_user.Id);
        
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