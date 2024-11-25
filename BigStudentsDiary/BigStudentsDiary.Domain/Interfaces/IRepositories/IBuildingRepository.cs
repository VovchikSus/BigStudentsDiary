using BigStudentsDiary.Core.Interfaces;
using BigStudentsDiary.Domain.Models;

namespace BigStudentsDiary.Domain.Interfaces.IRepositories;

public interface IBuildingRepository
{
    /// <summary>
    /// Возвращает список всех Строений, соответствующих условию, заданному функцией
    /// </summary>
    Task<IOperationResult<IEnumerable<Buildings>>> GetAllAsync(Func<Buildings, bool> selectFunc = null);
}