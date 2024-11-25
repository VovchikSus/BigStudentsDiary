using BigStudentsDiary.Domain.Models;
using Microsoft.Data.SqlClient;

namespace BigStudentsDiary.Infrastructure.CreationObjectFromSql;

public class GroupCreator : ICreator<Groups>
{
    public Groups Map(SqlDataReader reader)
    {
        return Groups.Create(
            int.Parse(reader["GroupId"].ToString()),
            reader["GroupCode"].ToString());
    }
}
