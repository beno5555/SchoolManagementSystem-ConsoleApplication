using ProjectHelperLibrary.Response;
using ProjectHelperLibrary.Utilities;
using SchoolManagementSystem.Data;
using SchoolManagementSystem.Data.Models;
using SchoolManagementSystem.Data.Repositories;

namespace SchoolManagementSystem;

public class Program
{
    public class UserService
    {
        private readonly UserRepository _userRepository = new();
        private readonly SubjectRepository _subjectRepository = new();
        private readonly SubjectEnrollmentRepository _subjectEnrollmentRepository = new();
        private readonly SchoolClassRepository _schoolClassRepository = new();
        public async  Task<DataResponse<List<User>>> GetAllUsers()
        {
            var response = await _userRepository.GetAll();
            if (!response.Success)
            {
                ConsoleUtilities.PrintError(response.Message);
            }

            return response;
        }

        public async Task<DataResponse<List<User>>> GetAllStudentsWithSubject(int subjectId)
        {
            var response = new DataResponse<List<User>>();
            var classesResponse = _schoolClassRepository.GetClassesBySubjectId(subjectId);
            
            if (classesResponse.Success)
            {
                var classIds = classesResponse.Value.Select(c => c.Id).ToList();
                var subjectEnrollments = _subjectEnrollmentRepository.GetSubjectEnrollmentsByClassIds(classIds);
                if (subjectEnrollments.Success)
                {
                    var students = await _userRepository.GetStudentsBySubjectEnrollments(subjectEnrollments.Value);
                    if (students.Success)
                    {
                        response.SetData(students.Value);
                    }
                    else
                    {
                        response.SetStatus(false, "No students found.");
                    }
                }
                else
                {
                    response.SetStatus(false, subjectEnrollments.Message);
                }
            }
            else
            {
                response.SetStatus(false, classesResponse.Message);
            }

            return response;
        }
        
    }
    private static async Task Main()
    {
        await SchoolContext.InitializeAsync();
        var userService = new UserService();        
        // var userRepository = new UserRepository();

        // var student = User.CreateStudent(
        //     "sandro",
        //     "benashvili",
        //     DateTime.Now,
        //     "34343",
        //     "aaa@gmail.com",
        //     "3433",
        //     2);
        // await userRepository.AddAsync(student);

        // var studentResponse = await userRepository.GetById(2);
        // if (studentResponse.Success)
        // {
        //     var deleteResponse = await userRepository.DeleteAsync(studentResponse.Value.Id);
        //     if (deleteResponse.Success)
        //     {
        //         Console.WriteLine("Successfully deleted");
        //     }
        //     else
        //     {
        //         ConsoleUtilities.PrintError(deleteResponse.Message);
        //     }
        // }
        // else
        // {
        //     ConsoleUtilities.PrintError(studentResponse.Message);
        // }


        int subjectId = SchoolContext.Subjects.First().Id;
        var studentsResponse = await userService.GetAllStudentsWithSubject(subjectId);
        if (studentsResponse.Success)
        {
            var students = studentsResponse.Value;
            PrintCollection(students);
        }
        else
        {
            ConsoleUtilities.PrintError(studentsResponse.Message);
        }
        
        // await FileManager.LoadAsync(AppConstants.FolderPaths.UserPath, schoolContext.Users);
        //
        // PrintCollection(schoolContext.Users);
        // ConsoleUtilities.WaitForKey(ConsoleKey.A);
        // schoolContext.Users.AddAsync(User.CreateStudent(
        //     "John", 
        //     "Doe", 
        //     new DateTime(2009, 08,16), 
        //     "454345434", 
        //     "alo@gmail.com", 
        //     "admin123", 
        //     1));
        // Console.WriteLine("added new student");
        // await FileManager.SaveAsync(
        //     AppConstants.FolderPaths.GetFullPath(AppConstants.FolderPaths.UserPath),
        //     schoolContext.Users);
        //
        // ConsoleUtilities.WaitForKey(ConsoleKey.A);
        //
        //
        // var menu = new Menu();
        // await menu.Run();

        // temporary
        Console.ReadKey();
    }

    static void PrintCollection(List<User> collection) 
    {
        if (collection.Count == 0)
        {
            Console.WriteLine("List is empty");
        }
        else
        {
            foreach (var item in collection)
            {
                Console.WriteLine($"{item.FullName}");
            }
        }
    }
}
