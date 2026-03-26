using SchoolManagementSystem.Data.Models;

namespace SchoolManagementSystem.Service.Display;

public static class DisplayManager
{
    public static void PrintCollection(List<User> collection)
    {
        if (collection.Count == 0)
            Console.WriteLine("List is empty");
        else
            foreach (var item in collection)
                Console.WriteLine($"{item.FullName}");
    }

    public static void PrintCollection()
    {
        
    }
}