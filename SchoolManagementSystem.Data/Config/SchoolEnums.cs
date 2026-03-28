namespace SchoolManagementSystem.Data.Config;

public static class SchoolEnums
{
    public enum RoleName
    {
        SuperAdmin = 1,
        Principal,
        Teacher,
        Student,
    }

    public enum PermissionName
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
        OverDue,
        Submitted,
    }
    
    public enum AssignmentTypeName
    {
        Homework = 1,
        Classwork,
        Test,
        Exam,
        Quiz,
        Project
    }
    
    // ???
    public enum UserStatus
    {

    }


    public enum SubjectName
    {
        Math = 1,
        Biology,
        ForeignLanguageEnglish,
        Georgian,
        Literature,
        ForeignLanguageRussian,
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

    public enum RoomTypeName
    {
        Classroom = 1,
        Office,
        Laboratory,
        ItDepartment,
        ConferenceRoom,
        LectureHall,
        Library,
        Cafeteria,
        Storage,
        Gym,
        Restroom
    }

    public enum SubmissionStatus
    {
        Submitted = 1,
        SubmittedLate,
        Reviewed,
        ReviewedLate
    }
}