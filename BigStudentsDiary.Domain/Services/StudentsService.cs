using BigStudentsDiary.Domain.Interfaces;
using BigStudentsDiary.Domain.Interfaces.Auth;
using BigStudentsDiary.Domain.Models;

namespace BigStudentsDiary.Domain.Services;

public class StudentsService
{
    private readonly IStudentsRepository _studentRepository;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IJwtProvider _jwtProvider;

    public StudentsService(IStudentsRepository studentsRepository, IPasswordHasher passwordHasher,
        IJwtProvider jwtProvider)
    {
        _studentRepository = studentsRepository;
        _passwordHasher = passwordHasher;
        _jwtProvider = jwtProvider;
    }

    public async Task Register(string studentName, string studentSurname, string studentLogin, string studentPassword,
        int groupId)
    {
        var hashedPassword = _passwordHasher.Generate(studentPassword);
        var student = Students.Create(Guid.NewGuid(), studentName, studentSurname, studentLogin, hashedPassword,
            groupId); // Используйте hashedPassword

        await _studentRepository.AddStudent(student);
    }

    public async Task<string> Login(string login, string password)
    {
        var student = await _studentRepository.GetByLoginAsync(login);
        if (student == null)
        {
            throw new UnauthorizedAccessException("Login failed: invalid login or password.");
        }

        Console.WriteLine($"Stored Password: {student.StudentPassword}"); // Логирование хеша пароля из базы
        Console.WriteLine($"Password being verified: {password}"); // Логирование входного пароля

        var result = _passwordHasher.Verify(password, student.StudentPassword);
        if (!result)
        {
            throw new UnauthorizedAccessException("Login failed: invalid login or password.");
        }

        var token = _jwtProvider.GenerateToken(student);
        return token;
    }
}
