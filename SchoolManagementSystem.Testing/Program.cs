using SchoolManagementSystem.Data.HelperClasses;
using SchoolManagementSystem.Data.Models;

namespace SchoolManagementSystem.Testing;
class Program
{
    static async Task Main()
    {
        await TestDeserializedJsonCollections();
        Console.ReadKey();
    }

    public static async Task TestDeserializedJsonCollections()
    {
        Console.WriteLine("=== JSON Deserialization ID Test ===");

        // --------------------------
        // Users
        // --------------------------
        var users = await FileManager.LoadAsync<User>(FolderPaths.UserPath);
        IdGenerator.InitializeId(users);
        var newUser = new User("TestUser", "testuserlastname", new DateTime(), "dddd", "2@gmail.com", "aaa", 5); // automatically assigns next ID
        users.Add(newUser);
        Console.WriteLine($"Created new User: {newUser.FullName} with ID {newUser.Id}");
        await FileManager.SaveAsync(FolderPaths.UserPath, users);

        // --------------------------
        // Courses
        // --------------------------
        var subjects = await FileManager.LoadAsync<Subject>(FolderPaths.SubjectPath);
        IdGenerator.InitializeId(subjects);
        var newCourse = new Subject("TestCourse"); // automatically assigns next ID
        subjects.Add(newCourse);
        Console.WriteLine($"Created new Course: {newCourse.SubjectName} with ID {newCourse.Id}");
        await FileManager.SaveAsync(FolderPaths.SubjectPath, subjects);

        // --------------------------
        // Add more model types here...
        // --------------------------
    }
    
}