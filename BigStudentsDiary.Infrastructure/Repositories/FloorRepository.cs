using BigStudentsDiary.Core.Implementations;
using BigStudentsDiary.Core.Interfaces;
using BigStudentsDiary.Domain.Interfaces.IRepositories;
using BigStudentsDiary.Domain.Models;
using BigStudentsDiary.Infrastructure.CreationObjectFromSql;

namespace BigStudentsDiary.Infrastructure.Repositories;

public class FloorRepository: RepositoryBase, IFloorRepository
{
    public FloorRepository(IConfiguration configuration) : base(configuration)
    {
    }

    public async Task<IOperationResult<IEnumerable<Floors>>> GetAllAsync(Func<Floors, bool> selectFunc = null)
    {
        var result = await ExecuteQueryAsync<Floors, FloorCreator>("SELECT * FROM Floors");

        if (selectFunc == null)
        {
            return new Success<IEnumerable<Floors>>(result);
        }
        else
        {
            return new Success<IEnumerable<Floors>>(result.Where(selectFunc));
        }
    }
}