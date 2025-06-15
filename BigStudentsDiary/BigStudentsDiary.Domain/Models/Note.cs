using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

namespace BigStudentsDiary.Domain.Models;


public class Note
{
    [JsonConstructor]
    private Note(
        Guid noteId,
        Guid studentId,
        int disciplineId,
        int lessonNumber,
        string content,
        DateTime createdAt,
        DateTime updatedAt)
    {
        NoteId = noteId;
        StudentId = studentId;
        DisciplineId = disciplineId;
        LessonNumber = lessonNumber;
        Content = content;
        CreatedAt = createdAt;
        UpdatedAt = updatedAt;
    }
    [Column("NoteID")]
    public Guid NoteId { get; set; }
    
    [Column("StudentID")]
    public Guid StudentId { get; set; }
    
    [Column("DisciplineID")]
    public int DisciplineId { get; set; }
    public int LessonNumber { get; set; }
    public string Content { get; set; } = "";
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    public static Note Create(
        Guid noteId,
        Guid studentId,
        int disciplineId,
        int lessonNumber,
        string content,
        DateTime createdAt,
        DateTime updatedAt)
    {
        return new Note(
            noteId,
            studentId,
            disciplineId,
            lessonNumber,
            content,
            createdAt,
            updatedAt);
    }
}