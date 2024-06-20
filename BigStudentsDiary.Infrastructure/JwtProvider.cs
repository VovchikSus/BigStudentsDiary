using System.IdentityModel.Tokens.Jwt;
using System.Runtime.CompilerServices;
using System.Security.Claims;
using System.Text;
using BigStudentsDiary.Domain.Interfaces.Auth;
using BigStudentsDiary.Domain.Models;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace BigStudentsDiary.Infrastructure;

public class JwtProvider : IJwtProvider
{
    private readonly JwtOptions _options;

    public JwtProvider(IOptions<JwtOptions> options)
    {
        _options = options.Value;
    }

    public string GenerateToken(Students student)
    {
        var claims = new[]
        {
            new Claim("studentId", student.StudentId.ToString()),
            new Claim("groupId", student.GroupId.ToString()) 
        };

        var signingCredentials =
            new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_options.SecretKey)),
                SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            claims: claims,
            signingCredentials: signingCredentials,
            expires: DateTime.UtcNow.AddHours(_options.ExpiresHours));

        var tokenValue = new JwtSecurityTokenHandler().WriteToken(token);
        return tokenValue;
    }

    public string GenerateToken(Teachers teacher)
    {
        var claims = new[]
        {
            new Claim("teacherId", teacher.TeacherInternalId.ToString()),
        };

        var signingCredentials =
            new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_options.SecretKey)),
                SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            claims: claims,
            signingCredentials: signingCredentials,
            expires: DateTime.UtcNow.AddHours(_options.ExpiresHours));

        var tokenValue = new JwtSecurityTokenHandler().WriteToken(token);
        return tokenValue; 
    }
}


