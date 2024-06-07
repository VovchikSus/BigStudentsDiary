using BigStudentsDiary.Core.Interfaces;
using BigStudentsDiary.Domain.Models;

namespace BigStudentsDiary.Domain.Interfaces;

/// <summary>
/// Интерфейс для работы с группами
/// </summary>
public interface IGroupRepository
{
    /// <summary>
    /// Возвращает список всех групп, соответствующих условию, заданному функцией
    /// </summary>
    Task<IOperationResult<IEnumerable<Groups>>> GetAllAsync(Func<Groups, bool> selectFunc = null);

    /// <summary>
    /// Добавляет группу из переданного параметра
    /// </summary>
    /// <param name="group">Группа</param>
    Task<IOperationResult<int>> AddGroup(Groups group);

    /// <summary>
    /// Изменяет группу из переданного параметра
    /// </summary>
    /// <param name="group">Данные для изменения группы</param>
    Task<IOperationResult> EditGroup(Groups group);

    /// <summary>
    /// Удаляет группу с переданным id
    /// </summary>
    /// <param name="id">Id группы, которую нужно удалить</param>
    Task<IOperationResult> DeleteGroup(int id);

    /// <summary>
    /// Возвращает группу по переданному GroupCode
    /// </summary>
    /// <param name="groupCode">Код группы, которую нужно получить</param>
    Task<IOperationResult<Groups>> GetByGroupCodeAsync(string groupCode);
}
