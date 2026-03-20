namespace SchoolManagementSystem.Data.HelperClasses;

[AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
public sealed class FileNamePrefixAttribute : Attribute
{   
    public string Name { get; set; }

    public FileNamePrefixAttribute(string name)
    {
        Name = name;
    }
}