using BigStudentsDiary.Core.Implementations;
using BigStudentsDiary.Core.Interfaces;
using BigStudentsDiary.Domain.Interfaces;
using BigStudentsDiary.Domain.Models;
using BigStudentsDiary.Infrastructure.CreationObjectFromSql;
using Microsoft.Data.SqlClient;

namespace BigStudentsDiary.Infrastructure.Repositories;

public class HomeWorksRepository : RepositoryBase, IHomeWorksRepository
{
    public HomeWorksRepository(IConfiguration configuration) : base(configuration)
    {
    }

    public async Task<IOperationResult<IEnumerable<HomeWorks>>> GetAllAsync(Func<HomeWorks, bool> selectFunc = null)
    {
        var result = await ExecuteQueryAsync<HomeWorks, HomeWorkCreator>("SELECT * FROM [Homework]");

        if (selectFunc == null)
        {
            return new Success<IEnumerable<HomeWorks>>(result);
        }
        else
        {
            return new Success<IEnumerable<HomeWorks>>(result.Where(selectFunc));
        }
    }

    public async Task<IOperationResult<Guid>> AddHomeWork(HomeWorks homeWork)
    {
        if (homeWork == null)
        {
            throw new ArgumentNullException(nameof(homeWork));
        }

        var id = Guid.NewGuid();
        await this.ExecuteNonQueryAsync(
            $"INSERT INTO [Homework] (HomeWorkDescription, HomeWorkID) VALUES ('{homeWork.HomeWorkDescription}','{id}')");
            // new SqlParameter("@homeWorkDescription", homeWork.HomeWorkDescription),
            // new SqlParameter("@homeWorkID", id));

        return new Success<Guid>(id);
    }

    public async Task<IOperationResult> EditHomeWork(HomeWorks homeWork)
    {
        
        if (homeWork == null)
            throw new ArgumentNullException(nameof(homeWork));

        var existing = (await this.ExecuteQueryAsync<Students, StudentCreator>(
            "SELECT * FROM Students WHERE StudentId = @id", new SqlParameter("@id", homeWork.HomeWorkId))).FirstOrDefault();

        if (existing == null)
            return new ElementNotFound($"Студент с id {homeWork.HomeWorkId} не найден");
        

        await ExecuteNonQueryAsync(
            $"UPDATE [Homework] SET HomeWorkDescription='{homeWork.HomeWorkDescription}' WHERE HomeWorkID= '{homeWork.HomeWorkId}'");
            // new SqlParameter("@HomeWorkDescription", homeWork.HomeWorkDescription),
            // new SqlParameter("@HomeWorkID", homeWork.HomeWorkId));

        return new Success();
    }



    public async Task<IOperationResult> DeleteHomeWork(Guid id)
    {
        var existing = (await ExecuteQueryAsync<HomeWorks, HomeWorkCreator>(
            "SELECT * FROM [Homework] WHERE HomeWorkID = @HomeWorkID",
            new SqlParameter("@HomeWorkID", id))).FirstOrDefault();

        if (existing == null)
            return new ElementNotFound($"Не найдена ДЗ с id {id}");

        await ExecuteNonQueryAsync("DELETE FROM [Homework] WHERE HomeWorkID = @HomeWorkID",
            new SqlParameter("@HomeWorkID", id));

        return new Success();
    }
}