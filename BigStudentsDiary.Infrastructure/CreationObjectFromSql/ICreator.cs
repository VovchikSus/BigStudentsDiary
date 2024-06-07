using Microsoft.Data.SqlClient;
using Npgsql;

namespace BigStudentsDiary.Infrastructure.CreationObjectFromSql;

public interface ICreator<T> where T : class //интерфейс сборщика с методом собрать
{
    T Map(SqlDataReader reader);
}