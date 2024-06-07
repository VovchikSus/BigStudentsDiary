using BigStudentsDiary.Domain.Models;
using Microsoft.Data.SqlClient;

namespace BigStudentsDiary.Infrastructure.CreationObjectFromSql;

public class HomeWorkCreator : ICreator<HomeWorks>
{
    public HomeWorks Map(SqlDataReader reader)
    {
        return HomeWorks.Create(
            Guid.Parse(reader["HomeWorkId"].ToString()),
            reader["HomeWorkDescription"].ToString());
    }
}