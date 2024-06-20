using BigStudentsDiary.Core.Interfaces;
using BigStudentsDiary.Domain.Models;

namespace BigStudentsDiary.Domain.Interfaces.IRepositories;

public interface IHomeWorksRepository
{
    /// <summary>
    /// Возвращает список всех дз, соответствующих условию, заданному функцией
    /// </summary>
    Task<IOperationResult<IEnumerable<HomeWorks>>> GetAllAsync(Func<HomeWorks, bool> selectFunc = null);

    /// <summary>
    /// Добавляет преподавателя из переданного параметра
    /// </summary>
    /// <param name="homeWork">Домашнее задание</param>
    Task<IOperationResult<Guid>> AddHomeWork(HomeWorks homeWork);

    /// <summary>
    /// Изменяет преподавателя из переданного параметра
    /// </summary>
    /// <param name="homeWork">Данные для изменения Дз</param>
    Task<IOperationResult> EditHomeWork(HomeWorks homeWork);

    /// <summary>
    /// Удаляет дз с переданным id
    /// </summary>
    /// <param name="id">Id дз, которого надо удалить</param>
    Task<IOperationResult> DeleteHomeWork(Guid id);
}