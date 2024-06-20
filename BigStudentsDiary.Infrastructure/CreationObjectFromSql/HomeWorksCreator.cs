using BigStudentsDiary.Domain.Models;
using Microsoft.Data.SqlClient;

namespace BigStudentsDiary.Infrastructure.CreationObjectFromSql;

public class HomeWorkCreator : ICreator<HomeWorks>
{
    public HomeWorks Map(SqlDataReader reader)
    {
        return HomeWorks.Create(
            reader["HomeWorkDescription"].ToString(),
            Guid.Parse(reader["HomeWorkId"].ToString()));
    }
}