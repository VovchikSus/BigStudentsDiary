using BigStudentsDiary.Core.Interfaces;
using BigStudentsDiary.Domain.Models;

namespace BigStudentsDiary.Domain.Interfaces.IRepositories;

public interface IStudentsRepository
{
    /// <summary>
    /// Возвращает список всех студентов, соответствующих условию, заданному функцией
    /// </summary>
    Task<IOperationResult<IEnumerable<Students>>> GetAllAsync(Func<Students, bool> selectFunc = null);
    
   
    /// <summary>
    /// Добавляет студента из переданного параметра
    /// </summary>
    /// <param name="student">Студент</param>
    Task<IOperationResult<Guid>> AddStudent(Students student);

    /// <summary>
    /// Изменяет студента из переданного параметра
    /// </summary>
    /// <param name="student">Данные для изменения студента</param>
    Task<IOperationResult> EditStudent(Students student);

    /// <summary>
    /// Удаляет студента с переданным id
    /// </summary>
    /// <param name="id">Id студента, которого надо удалить</param>
    Task<IOperationResult> DeleteStudent(Guid id);

    Task<Students> GetByLoginAsync(string login);


}