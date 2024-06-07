using System.ComponentModel.DataAnnotations;

namespace BigStudentsDiary.WebAPI.Contracts;

public record LoginStudentRequest(
    [Required] string StudentLogin,
    [Required] string StudentPassword);