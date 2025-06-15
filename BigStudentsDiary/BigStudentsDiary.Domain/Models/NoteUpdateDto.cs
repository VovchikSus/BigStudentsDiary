namespace BigStudentsDiary.Domain.Models;

public class NoteUpdateDto
{
    public Guid StudentId { get; set; }
    public int DisciplineId { get; set; }
    public int LessonNumber { get; set; }
    public string NewContent { get; set; } = "";
}