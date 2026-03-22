using SchoolManagementSystem.Data.Config;
using SchoolManagementSystem.Data.HelperClasses;
using SchoolManagementSystem.Data.Models;
using SchoolManagementSystem.Data.Models.JoinedModels;

namespace SchoolManagementSystem.Data;

public class SchoolContext
{
    #region Properties
    
    // main    
    public List<User> Users { get; set; } = new(AppConstants.MaximumCount.Users);
    public List<Subject> Subjects { get; set; } = new(AppConstants.MaximumCount.Subjects);
    public List<Room> Rooms { get; set; } = new(AppConstants.MaximumCount.Rooms);
    public List<Role> Roles { get; set; } = new(AppConstants.MaximumCount.Roles);
    public List<Permission> Permissions { get; set; } = new(AppConstants.MaximumCount.Permissions);
    public List<Group> Groups { get; set; } = new(AppConstants.MaximumCount.Groups);
    
    // academic    
    public List<Assignment> Assignments { get; set; } = new(AppConstants.MaximumCount.Assignments);
    public List<Assessment> Assessments { get; set; } = new(AppConstants.MaximumCount.Assessments);
    
    // joined    
    public List<SubjectEnrollment> SubjectEnrollments { get; set; } = new(AppConstants.MaximumCount.SubjectEnrollments); 
    public List<RolePermission> RolePermissions { get; set; } = new(AppConstants.MaximumCount.RolePermissions);
    public List<SchoolClass> SchoolClasses { get; set; } = new();
    public List<GroupClass> GroupClasses { get; set; } = new();

    // types
    public List<RoomType> RoomTypes { get; set; } = new();
    public List<AssignmentType> AssignmentTypes { get; set; } = new(AppConstants.MaximumCount.AssignmentTypes);

    #endregion
    
    #region Initialization
    
    public async Task InitializeAsync()
    {
        await FileManager.EnsureFoldersExist();
        await SeedData();
        await InitializeIds();
    }

    // using for testing. might remove later if I come up with a way to dynamically initialize ids.
    private async Task InitializeIds()
    {
        await IdGenerator.InitializeId(Users);
        await IdGenerator.InitializeId(Subjects);
        await IdGenerator.InitializeId(Rooms);
        await IdGenerator.InitializeId(Roles);
        await IdGenerator.InitializeId(Permissions);
        await IdGenerator.InitializeId(Groups);
        
        await IdGenerator.InitializeId(Assessments);
        await IdGenerator.InitializeId(Assignments);
        
        await IdGenerator.InitializeId(SubjectEnrollments);
        await IdGenerator.InitializeId(SchoolClasses);
        await IdGenerator.InitializeId(GroupClasses);
        
        await IdGenerator.InitializeId(AssignmentTypes);
        await IdGenerator.InitializeId(RoomTypes);
    }

    #endregion

    #region Seeding

    private async Task SeedData()
    {
        await Seeder.SeedEnums(Roles, typeof(SchoolEnums.RoleName), name => new Role(name));
        await Seeder.SeedEnums(Permissions, typeof(SchoolEnums.PermissionName), name => new Permission(name));
        await Seeder.SeedEnums(Subjects, typeof(SchoolEnums.SubjectName), name => new Subject(name));
        await Seeder.SeedEnums(AssignmentTypes, typeof(SchoolEnums.AssignmentTypeName), name => new AssignmentType(name));
        await Seeder.SeedEnums(RoomTypes, typeof(SchoolEnums.RoomTypeName), name => new RoomType(name));
        
        await Seeder.SeedSuperAdmin(Users, Roles);
    }
    
    #endregion
    
    #region Use examples

    #region Student Final Grade
    
    public int GetStudentFinalGrade(int studentId)
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

    private IEnumerable<SubjectEnrollment> GetSubjectEnrollments(int studentId)
    {
        return SubjectEnrollments.Where(se => se.StudentId == studentId);
    }

    private List<IGrouping<int, Assessment>> GetSubjectEnrollmentAssessments(IEnumerable<int> subjectEnrollmentIds)
    {
        return Assessments
            .Where(assessment => subjectEnrollmentIds
                .Contains(assessment.Id))
            .GroupBy(assessment => assessment.SubjectEnrollmentId)
            .ToList();
    }

    private List<decimal> GetAverageGradePerSubject(List<IGrouping<int, Assessment>> subjectEnrollmentAssessments)
    {
        var averageGradePerSubject = subjectEnrollmentAssessments
            .Select(seag => seag
                .Average(sea => sea.GradeValue))
            .ToList();
        return averageGradePerSubject;
    }

    private int GetStudentFinalGrade(List<decimal> subjectAverageGrades)
    {
        var studentAverageGrade = subjectAverageGrades
            .Select(ag => (int)Math.Round(ag))
            .Average();
        return (int)Math.Round(studentAverageGrade);
    }
        
    #endregion
    
    #region Every teacher of the student

    public List<User> GetStudentTeachers(int studentId)
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

    private List<SchoolClass> GetSubjectEnrollmentClasses(List<SubjectEnrollment> subjectEnrollments)
    {
        return subjectEnrollments
            .Select(se => SchoolClasses.FirstOrDefault(c => c.Id == se.SchoolClassId))
            // .Where(c => c is not null)
            .OfType<SchoolClass>()
            .ToList();
    }

    private List<User> GetClassTeachers(List<SchoolClass> classes)
    {
        return classes
            .Select(c => Users.FirstOrDefault(user => user.Id == c.TeacherId))
            // .Where(user => user is not null)
            .OfType<User>()
            .ToList();
    }
    #endregion

    #endregion
}
