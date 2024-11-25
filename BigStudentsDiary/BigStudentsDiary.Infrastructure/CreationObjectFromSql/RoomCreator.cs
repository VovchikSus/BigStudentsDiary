using BigStudentsDiary.Domain.Models;
using Microsoft.Data.SqlClient;

namespace BigStudentsDiary.Infrastructure.CreationObjectFromSql;

public class RoomCreator : ICreator<Rooms>
{
    public Rooms Map(SqlDataReader reader)
    {
        return Rooms.Create(
            reader["Room"].ToString(),
            int.Parse(reader["RoomId"].ToString()));
    }
}