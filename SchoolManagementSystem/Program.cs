using SchoolManagementSystem.Service.BusinessLogic;

namespace SchoolManagementSystem;

public class Program
{
    private static async Task Main()
    {
        await Menu.Run();

        // await SchoolContext.Execute();
        // var userService = new UserService();
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


        // var subjectId = SchoolContext.Subjects.First().Id;
        // var studentsResponse = await userService.GetAllStudentsWithSubject(1);
        // if (studentsResponse.Success)
        // {
        //     List<UserDisplayDTO> students = studentsResponse.Value;
        //     // DisplayManager.PrintCollection(students);
        // }
        // else
        // {
        //     ConsoleUtilities.PrintError(studentsResponse.Message);
        // }

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

    
}