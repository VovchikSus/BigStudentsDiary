using BigStudentsDiary.Domain.Models;
using Microsoft.Data.SqlClient;

namespace BigStudentsDiary.Infrastructure.CreationObjectFromSql;

public class TimeTableCreator : ICreator<TimeTable>
{
    public TimeTable Map(SqlDataReader reader)
    {
        return TimeTable.Create(
            Guid.Parse(reader["LessonId"].ToString()),
            DateTime.Parse(reader["DayDate"].ToString()),
            reader["Number"].ToString(),
            int.Parse(reader["DisciplineId"].ToString()),
            int.Parse(reader["FloorId"].ToString()),
            reader["DayOfWeekName"].ToString(),
            DateTime.Parse(reader["TimeStart"].ToString()),
            DateTime.Parse(reader["TimeEnd"].ToString()),
            reader["TimeRange"].ToString(),
            Convert.ToBoolean(reader["IsSession"]),
            int.Parse(reader["GroupId"].ToString()),
            int.Parse(reader["BuildingId"].ToString()),
            int.Parse(reader["RoomId"].ToString()),
            int.Parse(reader["DepartmentId"].ToString()),
            int.Parse(reader["SemesterId"].ToString()),
            int.Parse(reader["TypeId"].ToString()),
            int.Parse(reader["TeacherInternalId"].ToString())
        );
    }
}