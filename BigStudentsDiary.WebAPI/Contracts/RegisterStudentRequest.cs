using System.ComponentModel.DataAnnotations;

namespace BigStudentsDiary.WebAPI.Contracts;

public record RegisterStudentRequest(
    [Required] string StudentName,
    [Required] string StudentSurname,
    [Required] string StudentLogin,
    [Required] string StudentPassword,
    [Required] int GroupId
);