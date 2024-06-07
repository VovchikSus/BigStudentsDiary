using BigStudentsDiary.Core.Interfaces;
using BigStudentsDiary.Domain.Models;

namespace BigStudentsDiary.Domain.Interfaces;

public interface ITimeTableRepository
{
    /// <summary>
    /// Возвращает список расписания, по группе
    /// </summary>
    Task<IOperationResult<IEnumerable<TimeTable>>> GetTimeTableByGroup(int groupId, DateTime dayDate);
    
    Task<IOperationResult<IEnumerable<TimeTable>>> GetTimeTableByGroup(int groupId);
    /// <summary>
    /// Возвращаетсписок расписания по преподавателю
    /// </summary>
    Task<IOperationResult<IEnumerable<TimeTable>>> GetTimeTableByTeacher(int teacherInternalId, DateTime dayDate);
    
    Task<IOperationResult<IEnumerable<TimeTable>>> GetTimeTableByTeacher(int teacherInternalId);


  
}