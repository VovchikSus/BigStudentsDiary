using BigStudentsDiary.Core.Interfaces;
using BigStudentsDiary.Domain.Interfaces.IRepositories;
using BigStudentsDiary.Domain.Models;

namespace BigStudentsDiary.Domain.Services;

public class NoteService
{
    private readonly INoteRepository _noteRepository;

    public NoteService(INoteRepository noteRepository)
    {
        _noteRepository = noteRepository;
    }

    public async Task<IOperationResult<Guid>> AddNote(Note note)
        => await _noteRepository.AddNote(note);

    public async Task<IOperationResult<IEnumerable<Note>>> GetNotesByDiscipline(Guid studentId, string discipline)
        => await _noteRepository.GetByStudentAndDiscipline(studentId, discipline);
    public async Task<IOperationResult<Note>> GetNoteByParams(
        Guid studentId, 
        int disciplineId, 
        int lessonNumber)
    {
        return await _noteRepository.GetByParams(
            studentId, 
            disciplineId, 
            lessonNumber
        );
    }
    public async Task<IOperationResult> UpdateNote(Note note)
        => await _noteRepository.UpdateNote(note);
}