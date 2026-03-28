using SchoolManagementSystem.Data.Config;
using SchoolManagementSystem.Data.HelperClasses;
using SchoolManagementSystem.Data.Models;
using SchoolManagementSystem.Data.Models.Academic;
using SchoolManagementSystem.Data.Models.JoinedModels;
using SchoolManagementSystem.Data.Models.Named;

namespace SchoolManagementSystem.Data;

public static class SchoolContext
{
    #region Properties
    
    // main    
    public static List<User> Users { get; set; } = new(AppConstants.MaximumCount.Users);
    public static List<Subject> Subjects { get; set; } = new(AppConstants.MaximumCount.Subjects);
    public static List<Room> Rooms { get; set; } = new(AppConstants.MaximumCount.Rooms);
    public static List<Role> Roles { get; set; } = new(AppConstants.MaximumCount.Roles);
    public static List<Permission> Permissions { get; set; } = new(AppConstants.MaximumCount.Permissions);
    public static List<Group> Groups { get; set; } = new(AppConstants.MaximumCount.Groups);
    
    // academic    
    public static List<Assignment> Assignments { get; set; } = new(AppConstants.MaximumCount.Assignments);
    public static List<Assessment> Assessments { get; set; } = new(AppConstants.MaximumCount.Assessments);
    
    // joined    
    public static List<SubjectEnrollment> SubjectEnrollments { get; set; } = new(AppConstants.MaximumCount.SubjectEnrollments); 
    public static List<RolePermission> RolePermissions { get; set; } = new(AppConstants.MaximumCount.RolePermissions);
    public static List<SchoolClass> SchoolClasses { get; set; } = new();
    public static List<GroupClass> GroupClasses { get; set; } = new();

    // types
    public static List<RoomType> RoomTypes { get; set; } = new();
    public static List<AssignmentType> AssignmentTypes { get; set; } = new(AppConstants.MaximumCount.AssignmentTypes);
    public static List<Submission> Submissions { get; set; } = new();

    #endregion
    
    #region Initialization
    public static async Task InitializeAsync()
    {
        await FileManager.EnsureFoldersExist();
        await SeedData();
    }
    #endregion

    #region Seeding
    private static async Task SeedData()
    {
        await Seeder.SeedEnums<SchoolEnums.RoleName, Role>(Roles);
        await Seeder.SeedEnums<SchoolEnums.PermissionName, Permission>(Permissions);
        await Seeder.SeedEnums<SchoolEnums.SubjectName, Subject>(Subjects);
        await Seeder.SeedEnums<SchoolEnums.AssignmentTypeName, AssignmentType>(AssignmentTypes);
        await Seeder.SeedEnums<SchoolEnums.RoomTypeName, RoomType>(RoomTypes);
        
        await Seeder.SeedSuperAdmin(Users, Roles);
    }
    #endregion
    
    #region Use examples

    #region Student Final Grade
    
    public static int GetStudentFinalGrade(int studentId)
    {
        int studentFinalGrade;
        var user = Users.FirstOrDefault(user => user.Id == studentId);
        if (user is not null)
        {
            var subjectEnrollmentIds = GetSubjectEnrollments(user.Id).Select(se => se.Id);
            var subjectEnrollmentAssessments = GetSubjectEnrollmentAssessments(subjectEnrollmentIds);

            var averageGradeForEachSubject = GetAverageGradePerSubject(subjectEnrollmentAssessments);
            studentFinalGrade = GetStudentFinalGrade(averageGradeForEachSubject);
        }
        else
        {
            studentFinalGrade = -1;
        }

        return studentFinalGrade;
    }

    private static IEnumerable<SubjectEnrollment> GetSubjectEnrollments(int studentId)
    {
        return SubjectEnrollments.Where(se => se.StudentId == studentId);
    }

    private static List<IGrouping<int, Assessment>> GetSubjectEnrollmentAssessments(IEnumerable<int> subjectEnrollmentIds)
    {
        return null;
        // return Assessments
        //     .Where(assessment => subjectEnrollmentIds
        //         .Contains(assessment.Id))
        //     .GroupBy(assessment => assessment.)
        //     .ToList();
    }

    private static List<decimal> GetAverageGradePerSubject(List<IGrouping<int, Assessment>> subjectEnrollmentAssessments)
    {
        var averageGradePerSubject = subjectEnrollmentAssessments
            .Select(seag => seag
                .Average(sea => sea.GradeValue))
            .ToList();
        return averageGradePerSubject;
    }

    private static int GetStudentFinalGrade(List<decimal> subjectAverageGrades)
    {
        var studentAverageGrade = subjectAverageGrades
            .Select(ag => (int)Math.Round(ag))
            .Average();
        return (int)Math.Round(studentAverageGrade);
    }
        
    #endregion
    
    #region Every teacher of the student

    public static List<User> GetStudentTeachers(int studentId)
    {
        List<User> studentTeachers = []; 
        var student = Users.FirstOrDefault(user => user.Id == studentId);
        if (student is not null)
        {
            var subjectEnrollments =  GetSubjectEnrollments(student.Id).ToList();
            var subjectEnrollmentClasses = GetSubjectEnrollmentClasses(subjectEnrollments);
            studentTeachers = GetClassTeachers(subjectEnrollmentClasses);
        }

        return studentTeachers;
    }

    private static List<SchoolClass> GetSubjectEnrollmentClasses(List<SubjectEnrollment> subjectEnrollments)
    {
        return subjectEnrollments
            .Select(se => SchoolClasses.FirstOrDefault(c => c.Id == se.SchoolClassId))
            .OfType<SchoolClass>()
            .ToList();
    }

    private static List<User> GetClassTeachers(List<SchoolClass> classes)
    {
        return classes
            .Select(c => Users.FirstOrDefault(user => user.Id == c.TeacherId))
            .OfType<User>()
            .ToList();
    }
    #endregion

    #endregion
}
