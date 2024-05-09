using BigStudentsDiary.Core.Implementations;
using BigStudentsDiary.Core.Interfaces;
using BigStudentsDiary.Domain.Interfaces;
using BigStudentsDiary.Domain.Models;
using Microsoft.Data.SqlClient;

namespace BigStudentsDiary.Infrastructure;

public class TeachersRepository : ITeachersRepository
{
    private readonly string connectionStringName = "MainConnectionString";
    readonly IConfiguration configuration;

    public TeachersRepository(IConfiguration configuration)
    {
        this.configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
    }

    public async Task<IOperationResult<Guid>> AddTeacher(Teachers teacher)
    {
        if (teacher == null)
        {
            throw new ArgumentNullException(nameof(teacher));
        }

        var id = Guid.NewGuid();

        await this.ExecuteNonQueryAsync(
            $"INSERT INTO [Teachers] (TeacherFIO, TeacherLogin, TeacherPassword, TeacherExternalID, TeacherInternalID) Values ('{teacher.TeacherFio}','{teacher.TeacherLogin}','{teacher.TeacherPassword}','{id}','{teacher.TeacherInternalId}',)");
        return new Success<Guid>(id);
    }

    public async Task<IOperationResult> EditTeacher(Teachers teacher)
    {
        if (teacher == null)
        {
            throw new ArgumentNullException(nameof(teacher));
        }

        var existing =
            (await this.ExecuteQueryAsync(
                $"SELECT * FROM [Teachers] WHERE TeacherExternalID='{teacher.TeacherExternalId}'")).FirstOrDefault();
        if (existing == null)
        {
            return new ElementNotFound($"Преподаватель с id {teacher.TeacherExternalId} не найден");
        }

        existing.TeacherPassword = teacher.TeacherPassword;
        await this.ExecuteNonQueryAsync(
            $"UPDATE [Teachers] SET TeacherPassword= '{existing.TeacherPassword}' WHERE TeacherExternalID= '{existing.TeacherExternalId}'");
        return new Success();
    }


    public async Task<IOperationResult> DeleteTeacher(Guid id)
    {
        var existing = (await this.ExecuteQueryAsync($"SELECT * FROM [Teachers] where TeacherExternalID='{id}'"))
            .FirstOrDefault();
        if (existing == null)
        {
            return new ElementNotFound($"Не найден преподаватель с таким id{id}");
        }

        return new Success();
    }


    public async Task<IOperationResult<IEnumerable<Teachers>>> GetAllAsync(Func<Teachers, bool> selectFunc = null)
    {
        var result = await this.ExecuteQueryAsync("SELECT * FROM [Teachers]");

        if (selectFunc == null)
        {
            return new Success<IEnumerable<Teachers>>(result);
        }

        else
        {
            return new Success<IEnumerable<Teachers>>(result.Where(selectFunc));
        }
    }


    private async Task ExecuteNonQueryAsync(string sql)
    {
        using (var connection = new SqlConnection(this.configuration.GetConnectionString(connectionStringName)))
        {
            connection.Open();
            using (var command = new SqlCommand(sql, connection))
            {
                await command.ExecuteNonQueryAsync();
            }
        }
    }

    private async Task<IEnumerable<Teachers>> ExecuteQueryAsync(string sql)
    {
        var result = new List<Teachers>();
        using (var connection = new SqlConnection(this.configuration.GetConnectionString(connectionStringName)))
        {
            connection.Open();
            using (var command = new SqlCommand(sql, connection))
            {
                var reader = await command.ExecuteReaderAsync();
                while (reader.Read())
                {
                    result.Add(new Teachers
                    {
                        TeacherExternalId = Guid.Parse(reader["TeacherExternalId"].ToString()),
                        TeacherFio = reader["TeacherFio"].ToString(),
                        TeacherInternalId = Int32.Parse(reader["TeacherInternalId"].ToString()),
                        TeacherLogin = reader["TeacherLogin"].ToString(),
                        TeacherPassword = reader["TeacherPassword"].ToString(),
                    });
                }
            }
        }

        return result;
    }
}