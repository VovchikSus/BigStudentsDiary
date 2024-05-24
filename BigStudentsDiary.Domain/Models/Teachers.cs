using System.ComponentModel.DataAnnotations;

namespace BigStudentsDiary.Domain.Models;

public class Teachers
{
    [Key] public Guid TeacherExternalId { get; set; }
    public string TeacherFio { get; set; } = "";
    public string TeacherLogin { get; set; } = "";
    public string TeacherPassword { get; set; } = "";
    [Key] public int TeacherInternalId { get; set; }
}