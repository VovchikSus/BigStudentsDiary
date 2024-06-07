using BigStudentsDiary.Core.Implementations;
using BigStudentsDiary.Core.Interfaces;
using BigStudentsDiary.Domain.Interfaces;
using BigStudentsDiary.Infrastructure.CreationObjectFromSql;
using Microsoft.Data.SqlClient;

namespace BigStudentsDiary.Infrastructure;

public abstract class RepositoryBase
{
    private readonly string _connectionString;

    protected RepositoryBase(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("MainConnectionString")
                            ?? throw new ArgumentNullException(nameof(configuration));
    }

    protected async Task<IEnumerable<T>> ExecuteQueryAsync<T, TCreator>(string sql, params SqlParameter[] parameters)
        where T : class where TCreator : ICreator<T>, new()
    {
        var result = new List<T>();
        var creator = new TCreator();
        await using var connection = new SqlConnection(_connectionString);
        await connection.OpenAsync();
        await using (var command = new SqlCommand(sql, connection))
        {
            command.Parameters.AddRange(parameters);
            using var reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                result.Add(creator.Map(reader));
            }
        }

        await connection.CloseAsync();
        return result;
    }

    protected async Task ExecuteNonQueryAsync(string sql, params SqlParameter[] parameters)
    {
        if (string.IsNullOrEmpty(sql))
        {
            throw new ArgumentNullException(nameof(sql));
        }

        await using var connection = new SqlConnection(_connectionString);
        await connection.OpenAsync();
        await using (var command = new SqlCommand(sql, connection))
        {
            command.Parameters.AddRange(parameters);
            await command.ExecuteNonQueryAsync();
        }

        await connection.CloseAsync();
    }
}