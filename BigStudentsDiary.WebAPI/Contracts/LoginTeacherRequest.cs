using System.ComponentModel.DataAnnotations;

namespace BigStudentsDiary.WebAPI.Contracts;

public record LoginTeacherRequest(
    [Required] string TeacherLogin,
    [Required] string TeacherPassword);