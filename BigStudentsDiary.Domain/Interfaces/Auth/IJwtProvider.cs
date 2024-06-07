using BigStudentsDiary.Domain.Models;

namespace BigStudentsDiary.Domain.Interfaces.Auth;

public interface IJwtProvider
{
    public string GenerateToken(Students student);
}