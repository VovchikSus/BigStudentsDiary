using BigStudentsDiary.Core.Interfaces;
using BigStudentsDiary.Domain.Models;

namespace BigStudentsDiary.Domain.Interfaces.IRepositories;

public interface IDisciplinesRepository
{
    /// <summary>
    /// Возвращает список всех дисциплин, соответствующих условию, заданному функцией
    /// </summary>
    Task<IOperationResult<IEnumerable<Disciplines>>> GetAllAsync(Func<Disciplines, bool> selectFunc = null);
}