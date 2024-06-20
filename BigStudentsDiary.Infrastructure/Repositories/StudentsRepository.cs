using BigStudentsDiary.Core.Implementations;
using BigStudentsDiary.Core.Interfaces;
using BigStudentsDiary.Domain.Interfaces;
using BigStudentsDiary.Domain.Interfaces.IRepositories;
using BigStudentsDiary.Domain.Models;
using BigStudentsDiary.Infrastructure.CreationObjectFromSql;
using Microsoft.Data.SqlClient;

namespace BigStudentsDiary.Infrastructure.Repositories;

public class StudentsRepository : RepositoryBase, IStudentsRepository
{
    public StudentsRepository(IConfiguration configuration) : base(configuration)
    {
    }

    public async Task<IOperationResult<Guid>> AddStudent(Students student)
    {
        if (student == null)
            throw new ArgumentNullException(nameof(student));

        var id = Guid.NewGuid();
        await this.ExecuteNonQueryAsync(
            "INSERT INTO Students (studentId, name, surname, studentLogin, studentPassword, groupID) VALUES (@id, @name, @surname, @studentLogin, @studentPassword, @groupID)",
            new SqlParameter("@id", id),
            new SqlParameter("@name", student.Name),
            new SqlParameter("@surname", student.Surname),
            new SqlParameter("@studentLogin", student.StudentLogin),
            new SqlParameter("@studentPassword", student.StudentPassword),
            new SqlParameter("@groupID", student.GroupId));

        return new Success<Guid>(id);
    }

    public async Task<IOperationResult> DeleteStudent(Guid id)
    {
        var existing = (await this.ExecuteQueryAsync<Students, StudentCreator>(
            "SELECT * FROM Students WHERE StudentId = @id", new SqlParameter("@id", id))).FirstOrDefault();

        if (existing == null)
            return new ElementNotFound($"Не найден студент с id {id}");

        await this.ExecuteNonQueryAsync("DELETE FROM Students WHERE StudentId = @id", new SqlParameter("@id", id));
        return new Success();
    }

    public async Task<IOperationResult> EditStudent(Students student)
    {
        if (student == null)
            throw new ArgumentNullException(nameof(student));

        var existing = (await this.ExecuteQueryAsync<Students, StudentCreator>(
                "SELECT * FROM Students WHERE StudentId = @id", new SqlParameter("@id", student.StudentId)))
            .FirstOrDefault();

        if (existing == null)
            return new ElementNotFound($"Студент с id {student.StudentId} не найден");

        await this.ExecuteNonQueryAsync(
            "UPDATE Students SET Name = @name, Surname = @surname, StudentPassword = @studentPassword, StudentLogin = @studentLogin, GroupId = @groupID WHERE StudentId = @id",
            new SqlParameter("@name", student.Name),
            new SqlParameter("@surname", student.Surname),
            new SqlParameter("@studentPassword", student.StudentPassword),
            new SqlParameter("@studentLogin", student.StudentLogin),
            new SqlParameter("@groupID", student.GroupId),
            new SqlParameter("@id", student.StudentId));

        return new Success();
    }

    public async Task<IOperationResult<IEnumerable<Students>>> GetAllAsync(Func<Students, bool> selectFunc = null)
    {
        var result = await ExecuteQueryAsync<Students, StudentCreator>("SELECT * FROM Students");

        if (selectFunc == null)
        {
            return new Success<IEnumerable<Students>>(result);
        }
        else
        {
            return new Success<IEnumerable<Students>>(result.Where(selectFunc));
        }
    }

    public async Task<Students> GetByLoginAsync(string login)
    {
        var result = await this.ExecuteQueryAsync<Students, StudentCreator>(
            "SELECT * FROM Students WHERE StudentLogin = @login", new SqlParameter("@login", login));
        return result.FirstOrDefault();
    }
}