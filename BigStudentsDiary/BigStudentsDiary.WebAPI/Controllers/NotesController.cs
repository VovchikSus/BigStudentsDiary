using BigStudentsDiary.Domain.Models;
using BigStudentsDiary.Domain.Services;
using Microsoft.AspNetCore.Mvc;

namespace BigStudentsDiary.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class NotesController : ControllerBase
    {
        private readonly NoteService _noteService;

        public NotesController(NoteService noteService)
        {
            _noteService = noteService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateNote([FromBody] Note note)
        {
            var result = await _noteService.AddNote(note);
            return result.Successful 
                ? Ok(result.Result) 
                : BadRequest(result.ErrorMessage);
        }

        [HttpGet("{studentId}/{discipline}")]
        public async Task<IActionResult> GetNotes(Guid studentId, string discipline)
        {
            var result = await _noteService.GetNotesByDiscipline(studentId, discipline);
            return result.Successful 
                ? Ok(result.Result) 
                : NotFound(result.ErrorMessage);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateNote([FromBody] NoteUpdateDto dto)
        {
            // 1. Получаем существующую заметку
            var noteResult = await _noteService.GetNoteByParams(
                dto.StudentId, 
                dto.DisciplineId, 
                dto.LessonNumber
            );
    
            if (!noteResult.Successful)
                return NotFound(noteResult.ErrorMessage);

            // 2. Обновляем контент
            var note = noteResult.Result;
            note.Content = dto.NewContent;
            note.UpdatedAt = DateTime.UtcNow;

            // 3. Сохраняем изменения
            var updateResult = await _noteService.UpdateNote(note);
            return updateResult.Successful 
                ? Ok() 
                : BadRequest(updateResult.ErrorMessage);
        }
    }
}