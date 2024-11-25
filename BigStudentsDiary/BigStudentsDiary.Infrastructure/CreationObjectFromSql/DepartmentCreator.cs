using BigStudentsDiary.Domain.Models;
using Microsoft.Data.SqlClient;

namespace BigStudentsDiary.Infrastructure.CreationObjectFromSql;

public class DepartmentCreator:ICreator<Departments>
{
    public Departments Map(SqlDataReader reader)
    {
        return Departments.Create(
            Convert.ToInt32(reader["DepartmentId"]),
            reader["DepartmentCode"].ToString(),
            reader["DepartmentName"].ToString()
        );
    }
}