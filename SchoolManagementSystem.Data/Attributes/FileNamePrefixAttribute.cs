namespace SchoolManagementSystem.Data.Attributes;

/// <summary>
/// attributed used to dynamically create the JSON file names for each class
/// </summary>
[AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
public sealed class FileNamePrefixAttribute : Attribute
{   
    public string Name { get; }

    public FileNamePrefixAttribute(string name)
    {
        Name = name;
    }
}