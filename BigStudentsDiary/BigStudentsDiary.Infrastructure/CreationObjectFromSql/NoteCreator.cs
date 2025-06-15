using BigStudentsDiary.Domain.Models;
using Microsoft.Data.SqlClient;

namespace BigStudentsDiary.Infrastructure.CreationObjectFromSql;

public class NoteCreator : ICreator<Note>
{
    public Note Map(SqlDataReader reader)
    {
        return Note.Create(
            noteId: new Guid(reader["NoteId"].ToString()),
            studentId: new Guid(reader["StudentId"].ToString()),
            disciplineId: Convert.ToInt32(reader["DisciplineId"]),
            lessonNumber: Convert.ToInt32(reader["LessonNumber"]),
            content: reader["Content"].ToString(),
            createdAt: Convert.ToDateTime(reader["CreatedAt"]),
            updatedAt: Convert.ToDateTime(reader["UpdatedAt"])
        );
    }
}