using BigStudentsDiary.Core.Interfaces;
using BigStudentsDiary.Domain.Models;

namespace BigStudentsDiary.Domain.Interfaces.IRepositories;

public interface INoteRepository
{
    Task<IOperationResult<Guid>> AddNote(Note note);
    Task<IOperationResult> DeleteNote(Guid noteId);
    Task<IOperationResult> UpdateNote(Note note);
    Task<IOperationResult<IEnumerable<Note>>> GetAllAsync(Func<Note, bool> selectFunc = null);
    Task<IOperationResult<IEnumerable<Note>>> GetByStudentAndDiscipline(Guid studentId, string disciplineName);
    Task<IOperationResult<Note>> GetNoteById(Guid noteId);
    Task<IOperationResult<Note>> GetByParams(Guid studentId, int disciplineId, int lessonNumber);
    Task CreateEmptyNotesForStudent(Guid studentId, int groupId);
}