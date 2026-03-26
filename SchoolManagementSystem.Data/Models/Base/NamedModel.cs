namespace SchoolManagementSystem.Data.Models.Base;

public class NamedModel : BaseModel
{
    public string Name { get; set; } = string.Empty;

    public NamedModel()
    {
        
    }

    public NamedModel(string name)
    {
        Name = name;
    }
}