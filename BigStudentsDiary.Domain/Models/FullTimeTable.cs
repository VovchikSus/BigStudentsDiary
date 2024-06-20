namespace BigStudentsDiary.Domain.Models;

public class FullTimeTableDto
{
    public Guid LessonId { get; set; }
    public DateTime DayDate { get; set; }
    public string Number { get; set; } = "";
    public string Discipline { get; set; } = "";

    public string Department { get; set; } = "";
    public string Building { get; set; } = "";
    public string Room { get; set; } = "";
    public string DayOfWeekName { get; set; } = "";
    public DateTime TimeStart { get; set; }
    public DateTime TimeEnd { get; set; }
    public string TimeRange { get; set; } = "";
    public bool IsSession { get; set; }
    public int GroupId { get; set; }
    public int BuildingId { get; set; }
    public int RoomId { get; set; }
    public int DepartmentId { get; set; }
    public int SemesterId { get; set; }
    public int TypeId { get; set; }
    public int TeacherInternalId { get; set; }
}