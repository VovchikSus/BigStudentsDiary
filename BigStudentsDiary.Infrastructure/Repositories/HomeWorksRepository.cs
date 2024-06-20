using BigStudentsDiary.Core.Implementations;
using BigStudentsDiary.Core.Interfaces;
using BigStudentsDiary.Domain.Interfaces;
using BigStudentsDiary.Domain.Interfaces.IRepositories;
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
        var result = await ExecuteQueryAsync<HomeWorks, HomeWorkCreator>("SELECT * FROM Homework");

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
            $"INSERT INTO [Homework] (HomeWorkDescription, HomeWorkID ) VALUES ('{homeWork.HomeWorkDescription}','{id}')");
            //new SqlParameter("@homeWorkDescription", homeWork.HomeWorkDescription),
            //new SqlParameter("homeWorkID", id));

        return new Success<Guid>(id);
    }



    public async Task<IOperationResult> EditHomeWork(HomeWorks homeWork)
    {
        if (homeWork == null)
            throw new ArgumentNullException(nameof(homeWork));

        var existing = (await this.ExecuteQueryAsync<HomeWorks, HomeWorkCreator>(
                "SELECT * FROM Homework WHERE HomeWorkID = @id", new SqlParameter("@id", homeWork.HomeWorkId)))
            .FirstOrDefault();

        if (existing == null)
            return new ElementNotFound($"Домашнее задание с id { homeWork.HomeWorkId} не найден");

        await this.ExecuteNonQueryAsync(
            $"UPDATE [Homework] SET HomeWorkDescription='{existing.HomeWorkDescription}' WHERE HomeWorkID= '{existing.HomeWorkId}'");

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