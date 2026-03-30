namespace SchoolManagementSystem.Data.Models.Base;

public abstract class NamedModel : BaseModel
{
    public string Name { get; set; } = string.Empty;

    public virtual string GetName()
    {
        return Name;
    } 
}