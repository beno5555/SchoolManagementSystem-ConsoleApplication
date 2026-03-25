using SchoolManagementSystem.Data.Models;
using SchoolManagementSystem.Data.Repositories.Base;

namespace SchoolManagementSystem.Data.Repositories;

public class SubjectRepository : BaseRepository<Subject>
{
    public SubjectRepository() : base(SchoolContext.Subjects) 
    {
        
    }
}