using BigStudentsDiary.Core.Implementations;
using BigStudentsDiary.Core.Interfaces;
using BigStudentsDiary.Domain.Interfaces.IRepositories;
using BigStudentsDiary.Domain.Models;
using BigStudentsDiary.Infrastructure.CreationObjectFromSql;

namespace BigStudentsDiary.Infrastructure.Repositories;

public class DepartmentRepository: RepositoryBase,IDepartmentRepository
{
    public DepartmentRepository(IConfiguration configuration) : base(configuration)
    {
    }

    public async Task<IOperationResult<IEnumerable<Departments>>> GetAllAsync(Func<Departments, bool> selectFunc = null)
    {
        var result = await ExecuteQueryAsync<Departments, DepartmentCreator>("SELECT * FROM Departments");

        if (selectFunc == null)
        {
            return new Success<IEnumerable<Departments>>(result);
        }
        else
        {
            return new Success<IEnumerable<Departments>>(result.Where(selectFunc));
        }
    }
}