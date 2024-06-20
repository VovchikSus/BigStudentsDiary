using BigStudentsDiary.Domain.Models;
using Microsoft.Data.SqlClient;

namespace BigStudentsDiary.Infrastructure.CreationObjectFromSql;

public class DisciplineCreator:ICreator<Disciplines>
{
    public Disciplines Map(SqlDataReader reader)
    {
        return Disciplines.Create(
            reader["Discipline"].ToString(),
            Convert.ToInt32(reader["DisciplineId"])
        );
    }
}