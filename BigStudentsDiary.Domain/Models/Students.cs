namespace BigStudentsDiary.Domain.Models;

public class Students
{
    public Guid StudentId { get; set; }
    public string Name { get; set; } = "";
    public string Surname { get; set; } = "";
    public string StudentLogin { get; set; } = "";
    public string StudentPassword { get; set; } = "";
    public int GroupId { get; set; }
}