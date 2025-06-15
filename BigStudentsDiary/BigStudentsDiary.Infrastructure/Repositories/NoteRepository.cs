using BigStudentsDiary.Core.Implementations;
using BigStudentsDiary.Core.Interfaces;
using BigStudentsDiary.Domain.Interfaces.IRepositories;
using BigStudentsDiary.Domain.Models;
using BigStudentsDiary.Infrastructure.CreationObjectFromSql;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace BigStudentsDiary.Infrastructure.Repositories;

public class NoteRepository : RepositoryBase, INoteRepository
{
    private readonly IDisciplinesRepository _disciplinesRepo; // Добавьте это поле
    private readonly string _connectionString; 
    public NoteRepository(
        IConfiguration configuration,
        IDisciplinesRepository disciplinesRepo) // Конструктор с двумя параметрами
        : base(configuration)
    {
        _disciplinesRepo = disciplinesRepo; // Инициализируйте поле здесь
        _connectionString = configuration.GetConnectionString("MainConnectionString");
    }
    /*public async Task<IOperationResult<Guid>> AddNote(Note note)
    {
        try
        {
            if (note == null)
                throw new ArgumentNullException(nameof(note));

            using var connection = new SqlConnection(_connectionString);
        
            var noteId = Guid.NewGuid();
        
            await connection.ExecuteAsync(
                @"INSERT INTO Notes 
                (NoteID, StudentID, DisciplineID, LessonNumber, Content, CreatedAt, UpdatedAt) 
            VALUES 
                (@NoteId, @StudentId, @DisciplineId, @LessonNumber, @Content, GETUTCDATE(), GETUTCDATE())",
                new 
                {
                    NoteId = noteId,
                    note.StudentId,
                    note.DisciplineId,
                    note.LessonNumber,
                    note.Content
                });

            return new Success<Guid>(noteId);
        }
        catch (Exception ex)
        {
            // Логирование ошибки
            return new Failure<Guid>($"Ошибка при создании заметки: {ex.Message}");
        }
    }*/

    public async Task<IOperationResult<Guid>> AddNote(Note note)
    {
        if (note == null)
            throw new ArgumentNullException(nameof(note));
    
        var noteId = Guid.NewGuid();
    
        // Опасный подход! Не использовать в реальных проектах!
        await ExecuteNonQueryAsync(
            $@"
        INSERT INTO Notes 
            (NoteId, StudentId, DisciplineId, LessonNumber, Content, CreatedAt, UpdatedAt) 
        VALUES 
            (
                '{noteId}', 
                '{note.StudentId}', 
                {note.DisciplineId}, 
                {note.LessonNumber}, 
                N'{note.Content?.Replace("'", "''")}', 
                GETUTCDATE(), 
                GETUTCDATE()
            )"
        );
    
    
        return new Success<Guid>(noteId);
    }


    public async Task<IOperationResult> DeleteNote(Guid noteId)
    {
        var existing = (await ExecuteQueryAsync<Note, NoteCreator>(
                "SELECT * FROM Notes WHERE NoteId = @noteId",
                new SqlParameter("@noteId", noteId)))
            .FirstOrDefault();

        if (existing == null)
            return new ElementNotFound($"Заметка с ID {noteId} не найдена");

        await ExecuteNonQueryAsync(
            "DELETE FROM Notes WHERE NoteId = @noteId",
            new SqlParameter("@noteId", noteId));

        return new Success();
    }

    public async Task<IOperationResult<Note>> GetByParams(
        Guid studentId, 
        int disciplineId, 
        int lessonNumber)
    {
        const string sql = @"
        SELECT * FROM Notes 
        WHERE 
            StudentId = @studentId AND 
            DisciplineId = @disciplineId AND 
            LessonNumber = @lessonNumber";

        var result = await ExecuteQueryAsync<Note, NoteCreator>(
            sql,
            new SqlParameter("@studentId", studentId),
            new SqlParameter("@disciplineId", disciplineId),
            new SqlParameter("@lessonNumber", lessonNumber)
        );

        return result.Any() 
            ? new Success<Note>(result.First()) 
            : new ElementNotFound<Note>("Note not found");
    }
    
    
    public async Task<IOperationResult> UpdateNote(Note note)
    {
        if (note == null)
            throw new ArgumentNullException(nameof(note));

        // Опасный подход! Не использовать в реальных проектах!
        await ExecuteNonQueryAsync(
            $@"
        UPDATE Notes 
        SET 
            Content = N'{note.Content?.Replace("'", "''")}', 
            UpdatedAt = GETUTCDATE() 
        WHERE 
            NoteId = '{note.NoteId}'"
        );

        return new Success();
    }

    /*public async Task<IOperationResult> UpdateNote(Note note)
    {
        if (note == null) throw new ArgumentNullException(nameof(note));

        await ExecuteNonQueryAsync(
            "UPDATE Notes SET Content = @content, UpdatedAt = GETUTCDATE() WHERE NoteId = @noteId",
            new SqlParameter("@content", note.Content ?? (object)DBNull.Value),
            new SqlParameter("@noteId", note.NoteId)
        );

        return new Success();
    }*/
    
    public async Task<IOperationResult<IEnumerable<Note>>> GetAllAsync(Func<Note, bool> selectFunc = null)
    {
        var result = await ExecuteQueryAsync<Note, NoteCreator>("SELECT * FROM Notes");

        return selectFunc == null
            ? new Success<IEnumerable<Note>>(result)
            : new Success<IEnumerable<Note>>(result.Where(selectFunc));
    }

    public async Task<IOperationResult<IEnumerable<Note>>> GetByStudentAndDiscipline(
        Guid studentId,
        string disciplineName)
    {
        // 1. Получаем дисциплину через DisciplinesRepository
        var discipline = await _disciplinesRepo.GetDisciplineIdByName(disciplineName);

        if (discipline == null)
            return new ElementNotFound<IEnumerable<Note>>($"Дисциплина '{disciplineName}' не найдена");

        // 2. Ищем заметки
        var notes = await ExecuteQueryAsync<Note, NoteCreator>(
            "SELECT * FROM Notes WHERE StudentId = @StudentId AND DisciplineId = @DisciplineId",
            new SqlParameter("@StudentId", studentId),
            new SqlParameter("@DisciplineId", discipline.DisciplineId)
        );

        return new Success<IEnumerable<Note>>(notes);
    }
    
    public async Task<IOperationResult<Note>> GetNoteById(Guid noteId)
    {
        var note = (await ExecuteQueryAsync<Note, NoteCreator>(
                "SELECT * FROM Notes WHERE NoteId = @noteId",
                new SqlParameter("@noteId", noteId))
            ).FirstOrDefault();

        return note != null 
            ? new Success<Note>(note) 
            : new ElementNotFound<Note>("Заметка не найдена");
    }
    
    public async Task CreateEmptyNotesForStudent(Guid studentId, int groupId)
    {
        // Получаем дисциплины группы
        var disciplinesResult = await  _disciplinesRepo.GetDisciplinesByGroup(groupId);
        
        if (!disciplinesResult.Successful || disciplinesResult.Result == null) 
            return;
        
        foreach (var discipline in disciplinesResult.Result)
        {
            // Создаем пустые заметки для каждой дисциплины
            for (int lessonNumber = 1; lessonNumber <= discipline.TotalLessons; lessonNumber++)
            {
                var noteId = Guid.NewGuid();
                var content = "".Replace("'", "''");
                await ExecuteNonQueryAsync(
                    $@"INSERT INTO Notes 
                    (NoteId, StudentId, DisciplineId, LessonNumber, Content) 
                VALUES 
                    (
                        '{noteId}', 
                        '{studentId}', 
                        {discipline.DisciplineId}, 
                        {lessonNumber}, 
                        N'{content}'
                    )"
                );
                
            }
        }
    }
}