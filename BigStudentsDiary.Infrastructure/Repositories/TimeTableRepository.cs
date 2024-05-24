using BigStudentsDiary.Core.Implementations;
using BigStudentsDiary.Core.Interfaces;
using BigStudentsDiary.Domain.Interfaces;
using BigStudentsDiary.Domain.Models;
using Microsoft.Data.SqlClient;

namespace BigStudentsDiary.Infrastructure;

public class TimeTableRepository : ITimeTableRepository
{
    private readonly string connectionStringName = "MainConnectionString";
    readonly IConfiguration configuration;

    public TimeTableRepository(IConfiguration configuration)
    {
        this.configuration = configuration ??
                             throw new ArgumentNullException(nameof(configuration));
    }

    private async Task ExecuteNonQueryAsync(string sql)
    {
        using (var connection = new SqlConnection(
                   this.configuration.GetConnectionString(connectionStringName)))
        {
            connection.Open();
            using (var command = new SqlCommand(sql, connection))
                await command.ExecuteNonQueryAsync();
        }
    }

    /*private async Task<IEnumerable<TimeTable>> ExecuteQueryAsync(string sql)
    {
        var result = new List<TimeTable>();
        using (var connection = new SqlConnection(
                   this.configuration.GetConnectionString(connectionStringName)))
        {
            connection.Open();
            using (var command = new SqlCommand(sql, connection))
            {
                var reader = await command.ExecuteReaderAsync();
                while (reader.Read())
                {
                    result.Add(new TimeTable
                    {
                        LessonId = Guid.Parse(reader["LessonId"].ToString()),
                        DayDate = DateTime.Parse(reader["DayDate"].ToString()),
                        Number = reader["Number"].ToString(),
                        DisciplineId = Int32.Parse(reader["DisciplineId"].ToString()),
                        FloorId = Int32.Parse(reader["FloorId"].ToString()),
                        DayOfWeekName = reader["DayOfWeekName"].ToString(),
                        TimeStart = DateTime.Parse(reader["TimeStart"].ToString()),
                        TimeEnd = DateTime.Parse(reader["TimeEnd"].ToString()),
                        TimeRange = reader["TimeRange"].ToString(),
                        IsSession = Convert.ToBoolean(reader["IsSession"]),
                        GroupId = Int32.Parse(reader["GroupId"].ToString()),
                        BuildingId = Int32.Parse(reader["BuildingId"].ToString()),
                        RoomId = Int32.Parse(reader["RoomId"].ToString()),
                        DepartmentId = Int32.Parse(reader["DepartmentId"].ToString()),
                        SemesterId = Int32.Parse(reader["SemesterId"].ToString()),
                        TypeId = Int32.Parse(reader["TypeId"].ToString()),
                        TeacherInternalId = Int32.Parse(reader["TeacherInternalId"].ToString())
                    });
                }
            }
        }

        return result;
    }
*/
    private async Task<IEnumerable<TimeTable>> ExecuteQueryAsync(string sql, object parameters = null)
    {
        var result = new List<TimeTable>();
        using (var connection = new SqlConnection(this.configuration.GetConnectionString(connectionStringName)))
        {
            connection.Open();
            using (var command = new SqlCommand(sql, connection))
            {
                if (parameters != null)
                {
                    var properties = parameters.GetType().GetProperties();
                    foreach (var prop in properties)
                    {
                        command.Parameters.AddWithValue("@" + prop.Name, prop.GetValue(parameters));
                    }
                }

                var reader = await command.ExecuteReaderAsync();
                while (reader.Read())
                {
                    result.Add(new TimeTable
                    {
                        LessonId = Guid.Parse(reader["LessonId"].ToString()),
                        DayDate = DateTime.Parse(reader["DayDate"].ToString()),
                        Number = reader["Number"].ToString(),
                        DisciplineId = Int32.Parse(reader["DisciplineId"].ToString()),
                        FloorId = Int32.Parse(reader["FloorId"].ToString()),
                        DayOfWeekName = reader["DayOfWeekName"].ToString(),
                        TimeStart = DateTime.Parse(reader["TimeStart"].ToString()),
                        TimeEnd = DateTime.Parse(reader["TimeEnd"].ToString()),
                        TimeRange = reader["TimeRange"].ToString(),
                        IsSession = Convert.ToBoolean(reader["IsSession"]),
                        GroupId = Int32.Parse(reader["GroupId"].ToString()),
                        BuildingId = Int32.Parse(reader["BuildingId"].ToString()),
                        RoomId = Int32.Parse(reader["RoomId"].ToString()),
                        DepartmentId = Int32.Parse(reader["DepartmentId"].ToString()),
                        SemesterId = Int32.Parse(reader["SemesterId"].ToString()),
                        TypeId = Int32.Parse(reader["TypeId"].ToString()),
                        TeacherInternalId = Int32.Parse(reader["TeacherInternalId"].ToString())
                    });
                }
            }
        }

        return result;
    }

    public async Task<IOperationResult<IEnumerable<TimeTable>>> GetTimeTableByGroup(int groupId, DateTime dayDate)
    {
        var formatedDayDate = dayDate.Date.ToString("yyyy-MM-dd");
        var groupValue = groupId;
        var result = await this.ExecuteQueryAsync(
            "SELECT * FROM [TimeTable] WHERE GroupID = @groupId AND CONVERT(varchar, dayDate)  = @dayDate",
            new { groupId = groupValue, dayDate = formatedDayDate });

        return new Success<IEnumerable<TimeTable>>(result);
    }

    public async Task<IOperationResult<IEnumerable<TimeTable>>> GetTimeTableByGroup(int groupId)
    {
        var groupValue = groupId;
        var dayToday =
            DateTime.Now.Date.ToString("yyyy-MM-dd");
        var result = await this.ExecuteQueryAsync(
            "SELECT * FROM [TimeTable] WHERE GroupID = @groupId AND CONVERT(varchar, dayDate)  = @dayDate",
            new { groupId = groupValue, dayDate = dayToday });

        return new Success<IEnumerable<TimeTable>>(result);
    }

    public async Task<IOperationResult<IEnumerable<TimeTable>>> GetTimeTableByTeacher(int teacherInternalId, DateTime dayDate)
    {
        var teacherValue = teacherInternalId;
        var formatedDayDate = dayDate.Date.ToString("yyyy-MM-dd");
        var result = await this.ExecuteQueryAsync(
            "SELECT * FROM [TimeTable] WHERE TeacherInternalID = @teacherInternalId AND CONVERT(varchar, dayDate)  = @dayDate",
            new { teacherInternalId = teacherValue, dayDate = formatedDayDate });

        return new Success<IEnumerable<TimeTable>>(result);
    }

    public async Task<IOperationResult<IEnumerable<TimeTable>>> GetTimeTableByTeacher(int teacherInternalId)
    {
        var teacherValue = teacherInternalId;
        var dayToday =
            DateTime.Now.Date.ToString("yyyy-MM-dd");
        var result = await this.ExecuteQueryAsync(
            "SELECT * FROM [TimeTable] WHERE TeacherInternalID = @teacherInternalId AND CONVERT(varchar, dayDate)  = @dayDate",
            new { teacherInternalId = teacherValue, dayDate = dayToday });

        return new Success<IEnumerable<TimeTable>>(result);
    }
}