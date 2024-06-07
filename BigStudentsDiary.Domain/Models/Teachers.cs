using System.ComponentModel.DataAnnotations;

namespace BigStudentsDiary.Domain.Models;

public class Teachers
{
    [Key]
    public Guid TeacherExternalId { get; set; }
    public string TeacherFio { get; set; } = "";
    public string TeacherLogin { get; set; } = "";
    public string TeacherPassword { get; set; } = "";
    [Key]
    public int TeacherInternalId { get; set; }

    private Teachers(Guid teacherExternalId, string teacherFio, string teacherLogin, string teacherPassword, int teacherInternalId)
    {
        TeacherExternalId = teacherExternalId;
        TeacherFio = teacherFio;
        TeacherLogin = teacherLogin;
        TeacherPassword = teacherPassword;
        TeacherInternalId = teacherInternalId;
    }

    public static Teachers Create(Guid teacherExternalId, string teacherFio, string teacherLogin, string teacherPassword, int teacherInternalId)
    {
        return new Teachers(teacherExternalId, teacherFio, teacherLogin, teacherPassword, teacherInternalId);
    }
  
}