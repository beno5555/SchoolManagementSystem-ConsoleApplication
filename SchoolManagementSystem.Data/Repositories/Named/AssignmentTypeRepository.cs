using SchoolManagementSystem.Data.Models.Named;
using SchoolManagementSystem.Data.Repositories.Base;

namespace SchoolManagementSystem.Data.Repositories.Named;

public class AssignmentTypeRepository : NamedModelRepository<AssignmentType>
{
    public AssignmentTypeRepository() : base(SchoolContext.AssignmentTypes)
    {
    }
}