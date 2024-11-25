using BigStudentsDiary.Core.Interfaces;
using BigStudentsDiary.Domain.Models;

namespace BigStudentsDiary.Domain.Interfaces.IRepositories;

public interface IDepartmentRepository
{
    /// <summary>
    /// Возвращает список всех этажей, соответствующих условию, заданному функцией
    /// </summary>
    Task<IOperationResult<IEnumerable<Departments>>> GetAllAsync(Func<Departments, bool> selectFunc = null);
}