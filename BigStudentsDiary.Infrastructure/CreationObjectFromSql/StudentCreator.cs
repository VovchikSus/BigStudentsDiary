using BigStudentsDiary.Domain.Models;
using Microsoft.Data.SqlClient;
using Npgsql;

namespace BigStudentsDiary.Infrastructure.CreationObjectFromSql;

public class StudentCreator : ICreator<Students>
{
    public Students Map(SqlDataReader reader)
    {
        return Students.Create(
            Guid.Parse(reader["StudentId"].ToString()),    // Преобразование значения GUID из строки
            reader["Name"].ToString(),
            reader["Surname"].ToString(),
            reader["StudentLogin"].ToString(),
            reader["StudentPassword"].ToString(),
            Convert.ToInt32(reader["GroupId"])
        );
    }
}
