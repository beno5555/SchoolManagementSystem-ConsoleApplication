namespace SchoolManagementSystem.ConsoleDisplay;

public static class DisplayManager
{
    /// <summary>
    /// do not use this for UI
    /// </summary>
    public static void PrintCollection<T>(List<T> collection) where T : class
    {
        if (collection.Count == 0)
            Console.WriteLine("List is empty");
        else
        {
            foreach (var item in collection)
            {
                Print(item);
                
            }
        }
    }
    
    /// <summary>
    /// do not use this for UI
    /// </summary>
    public static void Print<T>(T objectToPrint, string divider = ": ") where T : class
    {
        var properties = objectToPrint.GetType().GetProperties();
        foreach (var property in properties)
        {
            string name = property.Name;
            var value = property.GetValue(objectToPrint);
            if (value is not null)
            {
                Console.WriteLine($"{name}{divider}{value}");
            }
        }
    }
}