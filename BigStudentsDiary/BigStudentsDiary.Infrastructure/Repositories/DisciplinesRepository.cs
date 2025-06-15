using BigStudentsDiary.Core.Implementations;
using BigStudentsDiary.Core.Interfaces;
using BigStudentsDiary.Domain.Interfaces.IRepositories;
using BigStudentsDiary.Domain.Models;
using BigStudentsDiary.Infrastructure.CreationObjectFromSql;
using Microsoft.Data.SqlClient;

namespace BigStudentsDiary.Infrastructure.Repositories;

public class DisciplinesRepository : RepositoryBase, IDisciplinesRepository
{
    public DisciplinesRepository(IConfiguration configuration) : base(configuration)
    {
    }

    public async Task<IOperationResult<IEnumerable<Disciplines>>> GetAllAsync(Func<Disciplines, bool> selectFunc = null)
    {
        var result = await ExecuteQueryAsync<Disciplines, DisciplineCreator>("SELECT * FROM Disciplines");

        if (selectFunc == null)
        {
            return new Success<IEnumerable<Disciplines>>(result);
        }
        else
        {
            return new Success<IEnumerable<Disciplines>>(result.Where(selectFunc));
        }
    }

    public async Task<Disciplines?> GetDisciplineIdByName(string disciplineName)
    {
        var result = await this.ExecuteQueryAsync<Disciplines, DisciplineCreator>(
            // Выбираем оба столбца
            "SELECT DisciplineID, Discipline FROM Disciplines WHERE Discipline = @DisciplineName",
            new SqlParameter("DisciplineName", disciplineName));
    
        return result.FirstOrDefault();
    }
    
    public async Task<IOperationResult<IEnumerable<DisciplineInfo>>> GetDisciplinesByGroup(int groupId)
    {
        try
        {
            var result = await ExecuteQueryAsync<DisciplineInfo, DisciplineInfoCreator>(
                @"SELECT d.DisciplineID, d.Discipline, COUNT(t.LessonID) AS TotalLessons 
            FROM TimeTable t
            JOIN Disciplines d ON t.DisciplineID = d.DisciplineID
            WHERE t.GroupID = @groupId
            GROUP BY d.DisciplineID, d.Discipline",
                new SqlParameter("@groupId", groupId));

            return new Success<IEnumerable<DisciplineInfo>>(result);
        }
        catch (Exception ex)
        {
            return new Failure<IEnumerable<DisciplineInfo>>($"Ошибка: {ex.Message}");
        }
    }
}