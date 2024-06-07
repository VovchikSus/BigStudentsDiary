using BigStudentsDiary.Domain.Models;
using Microsoft.Data.SqlClient;

namespace BigStudentsDiary.Infrastructure.CreationObjectFromSql;

public class TeacherCreator : ICreator<Teachers>
{
    public Teachers Map(SqlDataReader reader)
    {
        return Teachers.Create(
            Guid.Parse(reader["TeacherExternalId"].ToString()),
            reader["TeacherFio"].ToString(),
            reader["TeacherLogin"].ToString(),
            reader["TeacherPassword"].ToString(),
            int.Parse(reader["TeacherInternalId"].ToString())
        );
    }
}