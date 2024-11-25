using BigStudentsDiary.Domain.Models;
using Microsoft.Data.SqlClient;

namespace BigStudentsDiary.Infrastructure.CreationObjectFromSql;

public class FloorCreator : ICreator<Floors>
{
    public Floors Map(SqlDataReader reader)
    {
        return Floors.Create(
            reader["Floor"].ToString(),
            Convert.ToInt32(reader["FloorId"])
        );
    }
}