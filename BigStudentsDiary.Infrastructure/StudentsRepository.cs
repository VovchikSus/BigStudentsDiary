using BigStudentsDiary.Core.Implementations;
using BigStudentsDiary.Core.Interfaces;
using BigStudentsDiary.Domain.Interfaces;
using BigStudentsDiary.Domain.Models;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace BigStudentsDiary.Infrastructure;

public class StudentsRepository : IStudentsRepository
{
    private readonly string connectionStringName = "MainConnectionString";
    readonly IConfiguration configuration;

    public StudentsRepository(IConfiguration configuration)
    {
        this.configuration = configuration ??
                             throw new ArgumentNullException(nameof(configuration));
    }

    public async Task<IOperationResult<Guid>> AddStudent(Students student)
    {
        if (student == null)
            throw new ArgumentNullException(nameof(student));

        var id = Guid.NewGuid();
        await this.ExecuteNonQueryAsync(
            $"INSERT INTO [Students] (studentId, name, surname, studentLogin, studentPassword, groupID) VALUES ('{id}','{student.Name}','{student.Surname}','{student.StudentLogin}','{student.StudentPassword}','{student.GroupId}')");

        return new Success<Guid>(id);
    }

    public async Task<IOperationResult> DeleteStudent(Guid id)
    {
        var existing = (await this.ExecuteQueryAsync($"SELECT * FROM [Students] WHERE StudentId='{id}'"))
            .FirstOrDefault();

        if (existing == null)
            return new ElementNotFound($"Не найден студент с id {id}");

        await this.ExecuteNonQueryAsync($"DELETE FROM [Students] WHERE StudentId='{id}'");

        return new Success();
    }

    public async Task<IOperationResult> EditStudent(Students student)
    {
        if (student == null)
            throw new ArgumentNullException(nameof(student));

        var existing = (await this.ExecuteQueryAsync($"SELECT * FROM [Students] WHERE StudentId='{student.StudentId}'"))
            .FirstOrDefault();

        if (existing == null)
            return new ElementNotFound($"Студент с id {student.StudentId} не найден");

        existing.Name = student.Name;

        await this.ExecuteNonQueryAsync(
            $"UPDATE [Students] SET Name='{existing.Name}' WHERE StudentId= '{existing.StudentId}'");

        return new Success();
    }
    
    
    public async Task<IOperationResult<IEnumerable<Students>>> GetAllAsync(Func<Students, bool> selectFunc = null)
    {
        var result = await this.ExecuteQueryAsync("SELECT * FROM [Students]");

        if (selectFunc == null)
        {
            return new Success<IEnumerable<Students>>(result); // Вернем все элементы, так как selectFunc не указан
        }
        else
        {
            return new Success<IEnumerable<Students>>(result.Where(selectFunc)); // Вернем отфильтрованные элементы, если selectFunc указан
        }
    }

    private async Task ExecuteNonQueryAsync(string sql)
    {
        using (var connection = new SqlConnection(
                   this.configuration.GetConnectionString(connectionStringName)))
        {
            connection.Open();
            using (var command = new SqlCommand(sql, connection))
                await command.ExecuteNonQueryAsync();
        }
    }

    private async Task<IEnumerable<Students>> ExecuteQueryAsync(string sql)
    {
        var result = new List<Students>();
        using (var connection = new SqlConnection(
                   this.configuration.GetConnectionString(connectionStringName)))
        {
            connection.Open();
            using (var command = new SqlCommand(sql, connection))
            {
                var reader = await command.ExecuteReaderAsync();
                while (reader.Read())
                {
                    result.Add(new Students
                    {
                        StudentId = Guid.Parse(reader["StudentID"].ToString()),
                        Name = reader["Name"].ToString(),
                        Surname = reader["Surname"].ToString(),
                        StudentLogin = reader["StudentLogin"].ToString(),
                        StudentPassword = reader["StudentPassword"].ToString(),
                        GroupId = Int32.Parse(reader["GroupId"].ToString())
                    });
                }
            }
        }

        return result;
    }
}