using System.ComponentModel.DataAnnotations;
using System.Runtime.InteropServices.JavaScript;

namespace BigStudentsDiary.Domain.Models;

public class TimeTable
{
    private TimeTable(Guid lessonId, DateTime dayDate, string number, int disciplineId, int floorId,
        string dayOfWeekName, DateTime timeStart, DateTime timeEnd, string timeRange, bool isSession,
        int groupId, int buildingId, int roomId, int departmentId, int semesterId, int typeId, int teacherInternalId)
    {
        LessonId = lessonId;
        DayDate = dayDate;
        Number = number;
        DisciplineId = disciplineId;
        FloorId = floorId;
        DayOfWeekName = dayOfWeekName;
        TimeStart = timeStart;
        TimeEnd = timeEnd;
        TimeRange = timeRange;
        IsSession = isSession;
        GroupId = groupId;
        BuildingId = buildingId;
        RoomId = roomId;
        DepartmentId = departmentId;
        SemesterId = semesterId;
        TypeId = typeId;
        TeacherInternalId = teacherInternalId;
    }

    public Guid LessonId { get; set; }
    public DateTime DayDate { get; set; }
    public string Number { get; set; } = "";
    public int DisciplineId { get; set; }
    public int FloorId { get; set; }
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

    public static TimeTable Create(Guid lessonId, DateTime dayDate, string number, int disciplineId, int floorId,
        string dayOfWeekName, DateTime timeStart, DateTime timeEnd, string timeRange, bool isSession,
        int groupId, int buildingId, int roomId, int departmentId, int semesterId, int typeId, int teacherInternalId)
    {
        return new TimeTable(lessonId, dayDate, number, disciplineId, floorId, dayOfWeekName, timeStart, timeEnd,
            timeRange, isSession, groupId, buildingId, roomId, departmentId, semesterId, typeId, teacherInternalId);
    }
}