using BigStudentsDiary.Core.Interfaces;
using BigStudentsDiary.Domain.Models;

namespace BigStudentsDiary.Domain.Interfaces;

public interface ITeachersRepository
{
    /// <summary>
    /// Возвращает список всех преподавателей, соответствующих условию, заданному функцией
    /// </summary>
    Task<IOperationResult<IEnumerable<Teachers>>> GetAllAsync(Func<Teachers, bool> selectFunc = null);

    /// <summary>
    /// Добавляет преподавателя из переданного параметра
    /// </summary>
    /// <param name="teacher">Преподаватель</param>
    Task<IOperationResult<Guid>> AddTeacher(Teachers teacher);

    /// <summary>
    /// Изменяет преподавателя из переданного параметра
    /// </summary>
    /// <param name="teacher">Данные для изменения студента</param>
    Task<IOperationResult> EditTeacher(Teachers teacher);

    /// <summary>
    /// Удаляет преподавателя с переданным id
    /// </summary>
    /// <param name="id">Id Преподавателя, которого надо удалить</param>
    Task<IOperationResult> DeleteTeacher(Guid id);
    
    
    Task<Teachers> GetByLoginAsync(string login);
}