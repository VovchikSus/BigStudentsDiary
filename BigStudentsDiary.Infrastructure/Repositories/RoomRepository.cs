using BigStudentsDiary.Core.Implementations;
using BigStudentsDiary.Core.Interfaces;
using BigStudentsDiary.Domain.Interfaces.IRepositories;
using BigStudentsDiary.Domain.Models;
using BigStudentsDiary.Infrastructure.CreationObjectFromSql;

namespace BigStudentsDiary.Infrastructure.Repositories;

public class RoomRepository:RepositoryBase, IRoomRepository
{
    public RoomRepository(IConfiguration configuration) : base(configuration)
    {
    }

    public async Task<IOperationResult<IEnumerable<Rooms>>> GetAllAsync(Func<Rooms, bool> selectFunc = null)
    {
        var result = await ExecuteQueryAsync<Rooms, RoomCreator>("SELECT * FROM Rooms");

        if (selectFunc == null)
        {
            return new Success<IEnumerable<Rooms>>(result);
        }
        else
        {
            return new Success<IEnumerable<Rooms>>(result.Where(selectFunc));
        }
    }
}