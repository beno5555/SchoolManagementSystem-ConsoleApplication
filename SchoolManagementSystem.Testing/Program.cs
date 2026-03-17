using System.Xml.Schema;
using SchoolManagementSystem.Data;
using SchoolManagementSystem.Data.HelperClasses;
using SchoolManagementSystem.Data.Models;
using SchoolManagementSystem.Data.Models.Base;

namespace SchoolManagementSystem.Testing;
class Program
{
    static async Task Main()
    {
        var schoolContext = new SchoolContext();
        await schoolContext.InitializeAsync();
        
        // for (int i = 0; i < 10; i++)
        // {
        //     Console.WriteLine("Press any key to start testing...");
        //     Console.ReadKey(intercept: true);
        //     await TestDeserializedJsonCollections();
        // }
        

        Console.WriteLine("press any key to exit..");
        Console.ReadKey(intercept: true);
    }

    private static void PrintList<T>(List<T> list) where T : BaseModel
    {
        if (list.Any())
        {
            foreach (var item in list)
            {
                Console.WriteLine($"Id: {item.Id}");
            }
        }
    }
    // public static async Task TestDeserializedJsonCollections()
    // {
    //     Console.WriteLine("=== JSON Deserialization ID Test ===");
    //     SchoolContext context = new();
    //     await context.InitializeAsync();
    //     
    //     // --------------------------
    //     // Users
    //     // --------------------------
    //     await FileManager.LoadAsync<User>(AppConstantsFolderPaths.UserPath, context.Users);
    //     Console.WriteLine("before adding a new user:");
    //     PrintList(context.Users);
    //     
    //     IdGenerator.InitializeId(context.Users);
    //     
    //     var newUser = new User("TestUser", "testuserlastname", new DateTime(), "dddd", "2@gmail.com", "aaa", 5); // automatically assigns next ID
    //     context.Users.Add(newUser);
    //     Console.WriteLine($"Created new User: {newUser.FullName} with ID {newUser.Id}");
    //
    //     Console.WriteLine("after adding a new user:");
    //     PrintList(context.Users);
    //
    //     Console.WriteLine("Saving changes to json for later loads..");
    //     await FileManager.SaveAsync(AppConstantsFolderPaths.UserPath, context.Users);
    //
    //     // --------------------------
    //     // Subjects
    //     // --------------------------
    //     await FileManager.LoadAsync(AppConstantsFolderPaths.SubjectPath, context.Subjects);
    //     Console.WriteLine("before removing a subject:");
    //     PrintList(context.Subjects);
    //     
    //     IdGenerator.InitializeId(context.Subjects);
    //     Console.WriteLine($"Max subject id before deletion: {IdGenerator.MaxIds[typeof(Subject)]}");
    //     
    //     Subject subjectToRemove = context.Subjects.FirstOrDefault(subject => subject.Id == IdGenerator.MaxIds[typeof(Subject)])!;
    //     context.Subjects.Remove(subjectToRemove);
    //     Console.WriteLine($"Deleted subject: {subjectToRemove.SubjectName} with ID {subjectToRemove.Id}");
    //
    //     Console.WriteLine("after removing a subject:");
    //     PrintList(context.Subjects);
    //     
    //     IdGenerator.InitializeId(context.Subjects);
    //
    //     Console.WriteLine($"Max subject id after deletion: {IdGenerator.MaxIds[typeof(Subject)]}" +
    //                       $"");
    //     Console.WriteLine("Saving changes to json for later loads..");
    //     await FileManager.SaveAsync(AppConstantsFolderPaths.SubjectPath, context.Subjects);
    //
    //     // --------------------------
    //     // Add more model types here...
    //     // --------------------------
    // }
    
}