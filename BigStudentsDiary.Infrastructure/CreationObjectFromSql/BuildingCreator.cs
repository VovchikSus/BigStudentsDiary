using BigStudentsDiary.Domain.Models;
using Microsoft.Data.SqlClient;

namespace BigStudentsDiary.Infrastructure.CreationObjectFromSql;

public class BuildingCreator : ICreator<Buildings>
{
    public Buildings Map(SqlDataReader reader)
    {
        return Buildings.Create(
            Convert.ToInt32(reader["BuildingId"]),
            reader["Building"].ToString()
        );
    }
}