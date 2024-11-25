using BigStudentsDiary.Core.Implementations;
using BigStudentsDiary.Core.Interfaces;
using BigStudentsDiary.Domain.Interfaces.IRepositories;
using BigStudentsDiary.Domain.Models;
using BigStudentsDiary.Infrastructure.CreationObjectFromSql;

namespace BigStudentsDiary.Infrastructure.Repositories;

public class DisciplinesRepository: RepositoryBase, IDisciplinesRepository
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
}