using System.ComponentModel.DataAnnotations;

namespace BigStudentsDiary.Domain.Models;

public class HomeWorks
{
    [Key] public Guid HomeWorkId { get; set; }
    public string HomeWorkDiscription { get; set; } = "";
}