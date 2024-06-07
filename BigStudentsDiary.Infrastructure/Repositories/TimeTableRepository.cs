using BigStudentsDiary.Core.Implementations;
using BigStudentsDiary.Core.Interfaces;
using BigStudentsDiary.Domain.Interfaces;
using BigStudentsDiary.Domain.Models;
using BigStudentsDiary.Infrastructure.CreationObjectFromSql;
using Microsoft.Data.SqlClient;

namespace BigStudentsDiary.Infrastructure;
public class TimeTableRepository : RepositoryBase, ITimeTableRepository
{
    public TimeTableRepository(IConfiguration configuration) : base(configuration)
    {
    }

    public async Task<IOperationResult<IEnumerable<TimeTable>>> GetTimeTableByGroup(int groupId, DateTime dayDate)
    {
        var formatedDayDate = dayDate.Date.ToString("yyyy-MM-dd");
        var result = await ExecuteQueryAsync<TimeTable, TimeTableCreator>(
            "SELECT * FROM [TimeTable] WHERE GroupID = @GroupID AND CONVERT(varchar, dayDate) = @DayDate",
            new SqlParameter("@GroupID", groupId),
            new SqlParameter("@DayDate", formatedDayDate));

        return new Success<IEnumerable<TimeTable>>(result);
    }

    public async Task<IOperationResult<IEnumerable<TimeTable>>> GetTimeTableByGroup(int groupId)
    {
        var dayToday = DateTime.Now.Date.ToString("yyyy-MM-dd");
        var result = await ExecuteQueryAsync<TimeTable, TimeTableCreator>(
            "SELECT * FROM [TimeTable] WHERE GroupID = @GroupID AND CONVERT(varchar, dayDate) = @DayDate",
            new SqlParameter("@GroupID", groupId),
            new SqlParameter("@DayDate", dayToday));

        return new Success<IEnumerable<TimeTable>>(result);
    }

    public async Task<IOperationResult<IEnumerable<TimeTable>>> GetTimeTableByTeacher(int teacherInternalId, DateTime dayDate)
    {
        var formatedDayDate = dayDate.Date.ToString("yyyy-MM-dd");
        var result = await ExecuteQueryAsync<TimeTable, TimeTableCreator>(
            "SELECT * FROM [TimeTable] WHERE TeacherInternalID = @TeacherInternalID AND CONVERT(varchar, dayDate) = @DayDate",
            new SqlParameter("@TeacherInternalID", teacherInternalId),
            new SqlParameter("@DayDate", formatedDayDate));

        return new Success<IEnumerable<TimeTable>>(result);
    }

    public async Task<IOperationResult<IEnumerable<TimeTable>>> GetTimeTableByTeacher(int teacherInternalId)
    {
        var dayToday = DateTime.Now.Date.ToString("yyyy-MM-dd");
        var result = await ExecuteQueryAsync<TimeTable, TimeTableCreator>(
            "SELECT * FROM [TimeTable] WHERE TeacherInternalID = @TeacherInternalID AND CONVERT(varchar, dayDate) = @DayDate",
            new SqlParameter("@TeacherInternalID", teacherInternalId),
            new SqlParameter("@DayDate", dayToday));

        return new Success<IEnumerable<TimeTable>>(result);
    }
}
