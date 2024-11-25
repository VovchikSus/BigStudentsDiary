using BigStudentsDiary.Core.Interfaces;
using BigStudentsDiary.Domain.Models;

namespace BigStudentsDiary.Domain.Interfaces.IRepositories;

public interface IFloorRepository
{
    /// <summary>
    /// Возвращает список всех этажей, соответствующих условию, заданному функцией
    /// </summary>
    Task<IOperationResult<IEnumerable<Floors>>> GetAllAsync(Func<Floors, bool> selectFunc = null);
}