using ProjectHelperLibrary.Utilities;
using SchoolManagementSystem.Data;
using SchoolManagementSystem.Data.HelperClasses;
using SchoolManagementSystem.Data.Models;
using SchoolManagementSystem.Data.Models.UserProfiles;

namespace SchoolManagementSystem;

public class Program
{
    private static async Task Main()
    {
        SchoolContext schoolContext = new();
        await schoolContext.InitializeAsync();
        schoolContext.StudentProfiles = await FileManager.LoadAsync<StudentProfile>(FolderPaths.StudentProfilePath);
        
        PrintCollection(schoolContext.StudentProfiles);
        ConsoleUtilities.WaitForKey(ConsoleKey.A);
        schoolContext.StudentProfiles.Add(new StudentProfile(
            "John", 
            "Doe", 
            new DateTime(2009, 08,16), 
            "454345434", 
            "alo@gmail.com", 
            "admin123", 
            1));
        Console.WriteLine("added new student");
        await FileManager.SaveAsync(
            FolderPaths.GetFullPath(FolderPaths.StudentProfilePath),
            schoolContext.StudentProfiles);
        
        ConsoleUtilities.WaitForKey(ConsoleKey.A);

        
        // var menu = new Menu();
        // await menu.Run();

        // temporary
        Console.ReadKey();
    }

    static void PrintCollection(List<StudentProfile> collection) 
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
