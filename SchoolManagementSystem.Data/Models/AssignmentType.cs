namespace SchoolManagementSystem.Data.Models;

public class AssignmentType : BaseModel
{
    public string Name { get; set; } 

    public AssignmentType(int id, string name) : base(id)
    {
        Name = name;
    }
}