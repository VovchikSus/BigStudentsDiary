using BigStudentsDiary.Core.Implementations;
using BigStudentsDiary.Core.Interfaces;
using BigStudentsDiary.Domain.Interfaces.IRepositories;
using BigStudentsDiary.Domain.Models;
using BigStudentsDiary.Infrastructure.CreationObjectFromSql;

namespace BigStudentsDiary.Infrastructure.Repositories;

public class BuildingRepository : RepositoryBase, IBuildingRepository
{
    public BuildingRepository(IConfiguration configuration) : base(configuration)
    {
    }

    public async Task<IOperationResult<IEnumerable<Buildings>>> GetAllAsync(Func<Buildings, bool> selectFunc = null)
    {
        var result = await ExecuteQueryAsync<Buildings, BuildingCreator>("SELECT * FROM Buildings");

        if (selectFunc == null)
        {
            return new Success<IEnumerable<Buildings>>(result);
        }
        else
        {
            return new Success<IEnumerable<Buildings>>(result.Where(selectFunc));
        }
    }
}