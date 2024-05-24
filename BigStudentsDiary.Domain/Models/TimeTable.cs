using System.ComponentModel.DataAnnotations;
using System.Runtime.InteropServices.JavaScript;

namespace BigStudentsDiary.Domain.Models;

public class TimeTable
{
    [Key] public Guid LessonId { get; set; }
    public DateTime DayDate { get; set; }
    public string Number { get; set; }
    public int DisciplineId { get; set; }
    public int FloorId { get; set; }
    public string DayOfWeekName { get; set; }
    public DateTime TimeStart { get; set; }
    public DateTime TimeEnd { get; set; }
    public string TimeRange { get; set; }
    public bool IsSession { get; set; }
    public int GroupId { get; set; }
    public int BuildingId { get; set; }
    public int RoomId { get; set; }
    public int DepartmentId { get; set; }
    public int SemesterId { get; set; }
    public int TypeId { get; set; }
    [Key] public int TeacherInternalId { get; set; }
}