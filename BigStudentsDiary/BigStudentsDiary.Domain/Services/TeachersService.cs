using BigStudentsDiary.Domain.Interfaces;
using BigStudentsDiary.Domain.Interfaces.Auth;
using BigStudentsDiary.Domain.Interfaces.IRepositories;
using BigStudentsDiary.Domain.Models;
using Microsoft.AspNetCore.Identity;

namespace BigStudentsDiary.Domain.Services;

public class TeachersService
{
    private readonly ITeachersRepository _teachersRepository;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IJwtProvider _jwtProvider;

    public TeachersService(ITeachersRepository teachersRepository, IPasswordHasher passwordHasher,
        IJwtProvider jwtProvider)
    {
        _teachersRepository = teachersRepository;
        _passwordHasher = passwordHasher;
        _jwtProvider = jwtProvider;
    }
    

    public async Task<string> Login(string login, string password)
    {
        var teachers = await _teachersRepository.GetByLoginAsync(login);
        if (teachers == null)
        {
            throw new UnauthorizedAccessException("Login failed: invalid login or password.");
        }

        Console.WriteLine($"Stored Password: {teachers.TeacherPassword}"); // Логирование хеша пароля из базы
        Console.WriteLine($"Password being verified: {password}"); // Логирование входного пароля

        var result = _passwordHasher.Verify(password, teachers.TeacherPassword);
        if (!result)
        {
            throw new UnauthorizedAccessException("Login failed: invalid login or password.");
        }

        var token = _jwtProvider.GenerateToken(teachers);
        return token;
    }
}