using System.ComponentModel.DataAnnotations;

namespace SchoolManagementSystem.Data.Models;

public class Laboratory
{
    private static int _idIncrement = 1;
    public int LaboratoryId { get; set; } = _idIncrement++;
    [MaxLength(50)]
    public string LaboratoryName { get; set; } = string.Empty;
    [MaxLength(1000)]
    public string Description { get; set; } = string.Empty;

    public List<Subject> Subjects { get; set; } = new(5);
}