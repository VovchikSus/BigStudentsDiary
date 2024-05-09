namespace BigStudentsDiary.Domain.Models;

public class Teachers
{
    public Guid TeacherExternalId { get; set; } 
    public string TeacherFio { get; set; } = "";
    public string TeacherLogin { get; set; } = "";
    public string TeacherPassword { get; set; } = "";
    public int TeacherInternalId { get; set; }
    
}