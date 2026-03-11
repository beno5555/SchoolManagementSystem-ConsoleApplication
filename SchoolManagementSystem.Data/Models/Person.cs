using ProjectHelperLibrary.Utilities;

namespace SchoolManagementSystem.Data.Models;

public class Person
{
    // temporary solution
    private static int _idIncrement = 1;
    public int PersonId { get; private set; } = _idIncrement++;
    public string FirstName { get; private set; }
    public string LastName { get; private set; }
    public string FullName => $"{FirstName.ToCapitalized().Value} {LastName.ToCapitalized().Value}";

    public User? User { get; set; }

    public Person(string firstName, string lastName)
    {
        FirstName = firstName;
        LastName = lastName;
    }
}