using SchoolManagementSystem.Data.HelperClasses;

namespace SchoolManagementSystem.Data.Models
{
    public class BaseModel
    {
        public int Id { get; set; }

        protected BaseModel()
        {
            Id = IdGenerator.Next(GetType());
        }
    }
}
