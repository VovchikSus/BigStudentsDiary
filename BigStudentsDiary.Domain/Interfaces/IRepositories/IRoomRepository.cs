using BigStudentsDiary.Core.Interfaces;
using BigStudentsDiary.Domain.Models;

namespace BigStudentsDiary.Domain.Interfaces.IRepositories;

public interface IRoomRepository
{
    /// <summary>
    /// Возвращает список всех Rooms, соответствующих условию, заданному функцией
    /// </summary>
    Task<IOperationResult<IEnumerable<Rooms>>> GetAllAsync(Func<Rooms, bool> selectFunc = null);
}