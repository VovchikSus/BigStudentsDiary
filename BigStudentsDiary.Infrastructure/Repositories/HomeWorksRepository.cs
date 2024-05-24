using BigStudentsDiary.Core.Implementations;
using BigStudentsDiary.Core.Interfaces;
using BigStudentsDiary.Domain.Interfaces;
using BigStudentsDiary.Domain.Models;
using Microsoft.Data.SqlClient;

namespace BigStudentsDiary.Infrastructure;

public class HomeWorksRepository : IHomeWorksRepository
{
    private readonly string connectionStringName = "MainConnectionString";
    readonly IConfiguration configuration;


    public HomeWorksRepository(IConfiguration configuration)
    {
        this.configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
    }
    public async Task<IOperationResult<IEnumerable<HomeWorks>>> GetAllAsync(Func<HomeWorks, bool> selectFunc = null)
    {
        var result = await this.ExecuteQueryAsync("SELECT * FROM [Homework]");

        if (selectFunc == null)
        {
            return new Success<IEnumerable<HomeWorks>>(result); 
        }
        else
        {
            return
                new Success<IEnumerable<HomeWorks>>(
                    result.Where(selectFunc)); 
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
            $"INSERT INTO [Homework] (HomeWorkDiscription, HomeWorkID ) VALUES ('{homeWork.HomeWorkDiscription}','{id}')");
        return new Success<Guid>(id);
    }

    public async Task<IOperationResult> EditHomeWork(HomeWorks homeWork)
    {
        if (homeWork == null)
            throw new ArgumentNullException(nameof(homeWork));

        var existing =
            (await this.ExecuteQueryAsync($"SELECT * FROM [Homework] WHERE HomeWorkID='{homeWork.HomeWorkId}'"))
            .FirstOrDefault();

        if (existing == null)
            return new ElementNotFound($"Дз с id {homeWork.HomeWorkId} не найден");

        existing.HomeWorkDiscription = homeWork.HomeWorkDiscription;


        await this.ExecuteNonQueryAsync(
            $"UPDATE [Homework] SET HomeWorkDiscription='{existing.HomeWorkDiscription}' WHERE HomeWorkID= '{existing.HomeWorkId}'");

        return new Success();
    }

    public async Task<IOperationResult> DeleteHomeWork(Guid id)
    {
        var existing = (await this.ExecuteQueryAsync($"SELECT * FROM [Homework] WHERE HomeWorkID='{id}'"))
            .FirstOrDefault();

        if (existing == null)
            return new ElementNotFound($"Не найдена ДЗ с id {id}");

        await this.ExecuteNonQueryAsync($"DELETE FROM [Homework] WHERE HomeWorkID='{id}'");

        return new Success();
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

    private async Task<IEnumerable<HomeWorks>> ExecuteQueryAsync(string sql)
    {
        var result = new List<HomeWorks>();
        using (var connection = new SqlConnection(
                   this.configuration.GetConnectionString(connectionStringName)))
        {
            connection.Open();
            using (var command = new SqlCommand(sql, connection))
            {
                var reader = await command.ExecuteReaderAsync();
                while (reader.Read())
                {
                    result.Add(new HomeWorks
                    {
                        HomeWorkId = Guid.Parse(reader["HomeWorkId"].ToString()),
                        HomeWorkDiscription = reader["HomeWorkDiscription"].ToString(),
                    });
                }
            }
        }

        return result;
    }
}