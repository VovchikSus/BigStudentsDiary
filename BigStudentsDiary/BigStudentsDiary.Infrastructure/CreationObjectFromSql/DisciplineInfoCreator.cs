using BigStudentsDiary.Domain.Models;
using Microsoft.Data.SqlClient;

namespace BigStudentsDiary.Infrastructure.CreationObjectFromSql;

public class DisciplineInfoCreator : ICreator<DisciplineInfo>
{
    public DisciplineInfo Map(SqlDataReader reader)
    {
        return new DisciplineInfo
        {
            DisciplineId = reader.GetInt32(reader.GetOrdinal("DisciplineId")),
            TotalLessons = reader.GetInt32(reader.GetOrdinal("TotalLessons"))
        };
    }
}