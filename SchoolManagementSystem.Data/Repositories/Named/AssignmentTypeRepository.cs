using SchoolManagementSystem.Data.Models.Named;
using SchoolManagementSystem.Data.Repositories.Base;

namespace SchoolManagementSystem.Data.Repositories.Named;

public class AssignmentTypeRepository : NamedModelRepository<AssignmentType>
{
    protected AssignmentTypeRepository() : base(SchoolContext.AssignmentTypes)
    {
    }
}