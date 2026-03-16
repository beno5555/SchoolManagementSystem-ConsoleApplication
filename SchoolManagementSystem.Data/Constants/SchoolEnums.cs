namespace SchoolManagementSystem.Data.Constants;

public static class SchoolEnums
{
    public enum RoleName
    {
        SuperAdmin = 1,
        Principal,
        Teacher,
        Student,
        Guest
    }

    public enum Permission
    {
        ViewGrade,
        ViewHomeworks,
        SubmitHomework,
        RateTeacher,

        GetOwnStudent,
        GetSubjects,
        UploadHomework,
        AssessStudent,
        ScheduleTest,

        GetAllMembers,
        GetAverageGradeOfAnyKind,
        AddMember,
        RemoveMember,
        GetMember,

        UpdateProfile
    }

    public enum AssignmentStatus
    {
        Pending = 0,
        Submitted,
        OverDue,
        Reviewed
    }
    
    // ???
    public enum UserStatus
    {
        
    }

    public enum AssessmentType
    {
        Homework = 1,
        Classwork,
        Test,
        Exam,
        Quiz,
        Project
    }

    public enum SubjectName
    {
        Math = 1,
        Biology,
        ForeignLanguageEnglish,
        Georgian,
        Literature,
        ForeginLanguageRussian,
        Physics,
        Chemistry,
        Geography,
        History,
        PhysicalEducation,
        Art,
        Music,
        
        WorldHistory,
        GeorgianHistory,
        CivicEducation,
        InformationTechnology,
        Robotics,
        HealthEducation,
        
        
    }
}