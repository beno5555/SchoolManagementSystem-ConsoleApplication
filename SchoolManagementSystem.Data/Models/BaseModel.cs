namespace SchoolManagementSystem.Data.Models
{
    public class BaseModel
    {
        public int Id { get; set; }

        public BaseModel(int id)
        {
            Id = id;
        }
    }
}
