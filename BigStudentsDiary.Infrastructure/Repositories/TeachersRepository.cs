using BigStudentsDiary.Core.Implementations;
using BigStudentsDiary.Core.Interfaces;
using BigStudentsDiary.Domain.Interfaces;
using BigStudentsDiary.Domain.Models;
using BigStudentsDiary.Infrastructure.CreationObjectFromSql;
using Microsoft.Data.SqlClient;

namespace BigStudentsDiary.Infrastructure.Repositories;

public class TeachersRepository : RepositoryBase, ITeachersRepository
{
    public TeachersRepository(IConfiguration configuration) : base(configuration)
    {
    }

    public async Task<IOperationResult<Guid>> AddTeacher(Teachers teacher)
    {
        if (teacher == null)
        {
            throw new ArgumentNullException(nameof(teacher));
        }

        var id = Guid.NewGuid();
        await ExecuteNonQueryAsync(
            "INSERT INTO [Teachers] (TeacherFIO, TeacherLogin, TeacherPassword, TeacherExternalID, TeacherInternalID) VALUES (@TeacherFIO, @TeacherLogin, @TeacherPassword, @TeacherExternalID, @TeacherInternalID)",
            new SqlParameter("@TeacherFIO", teacher.TeacherFio),
            new SqlParameter("@TeacherLogin", teacher.TeacherLogin),
            new SqlParameter("@TeacherPassword", teacher.TeacherPassword),
            new SqlParameter("@TeacherExternalID", id),
            new SqlParameter("@TeacherInternalID", teacher.TeacherInternalId));

        return new Success<Guid>(id);
    }

    public async Task<IOperationResult> EditTeacher(Teachers teacher)
    {
        if (teacher == null)
        {
            throw new ArgumentNullException(nameof(teacher));
        }

        var existing = (await ExecuteQueryAsync<Teachers, TeacherCreator>(
            "SELECT * FROM [Teachers] WHERE TeacherExternalID = @TeacherExternalID",
            new SqlParameter("@TeacherExternalID", teacher.TeacherExternalId))).FirstOrDefault();

        if (existing == null)
        {
            return new ElementNotFound($"Преподаватель с id {teacher.TeacherExternalId} не найден");
        }

        await ExecuteNonQueryAsync(
            "UPDATE [Teachers] SET TeacherPassword = @TeacherPassword WHERE TeacherExternalID = @TeacherExternalID",
            new SqlParameter("@TeacherPassword", teacher.TeacherPassword),
            new SqlParameter("@TeacherExternalID", teacher.TeacherExternalId));

        return new Success();
    }

    public async Task<IOperationResult> DeleteTeacher(Guid id)
    {
        var existing = (await ExecuteQueryAsync<Teachers, TeacherCreator>(
            "SELECT * FROM [Teachers] WHERE TeacherExternalID = @TeacherExternalID",
            new SqlParameter("@TeacherExternalID", id))).FirstOrDefault();

        if (existing == null)
        {
            return new ElementNotFound($"Не найден преподаватель с таким id {id}");
        }

        await ExecuteNonQueryAsync("DELETE FROM [Teachers] WHERE TeacherExternalID = @TeacherExternalID",
            new SqlParameter("@TeacherExternalID", id));

        return new Success();
    }

    public async Task<Teachers> GetByLoginAsync(string login)
    {
        var result = await this.ExecuteQueryAsync<Teachers, TeacherCreator>(
            "SELECT * FROM Teachers WHERE TeacherLogin = @login", new SqlParameter("@login", login));
        return result.FirstOrDefault();
    }

    public async Task<IOperationResult<IEnumerable<Teachers>>> GetAllAsync(Func<Teachers, bool> selectFunc = null)
    {
        var result = await ExecuteQueryAsync<Teachers, TeacherCreator>("SELECT * FROM [Teachers]");

        if (selectFunc == null)
        {
            return new Success<IEnumerable<Teachers>>(result);
        }
        else
        {
            return new Success<IEnumerable<Teachers>>(result.Where(selectFunc));
        }
    }
}
