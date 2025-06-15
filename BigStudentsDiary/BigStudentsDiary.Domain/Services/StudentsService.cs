using System.Transactions;
using BigStudentsDiary.Domain.Interfaces;
using BigStudentsDiary.Domain.Interfaces.Auth;
using BigStudentsDiary.Domain.Interfaces.IRepositories;
using BigStudentsDiary.Domain.Models;
using Microsoft.Data.SqlClient;

namespace BigStudentsDiary.Domain.Services;

public class StudentsService
{
    private readonly IStudentsRepository _studentRepository;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IJwtProvider _jwtProvider;
    private readonly INoteRepository _noteRepository;

    public StudentsService(IStudentsRepository studentsRepository, IPasswordHasher passwordHasher,
        INoteRepository noteRepository,
        IJwtProvider jwtProvider)

    {
        _studentRepository = studentsRepository;
        _passwordHasher = passwordHasher;
        _jwtProvider = jwtProvider;
        _noteRepository = noteRepository;
    }

    public async Task Register(string studentName, string studentSurname, string studentLogin, string studentPassword, int groupId)
    {
        using var transaction = new TransactionScope(
            TransactionScopeOption.Required,
            new TransactionOptions
            {
                IsolationLevel = IsolationLevel.Serializable,
                Timeout = TimeSpan.FromSeconds(60)
            },
            TransactionScopeAsyncFlowOption.Enabled
        );
        try
        {
            Guid studentId = Guid.NewGuid();
            var hashedPassword = _passwordHasher.Generate(studentPassword);
            var student = Students.Create(studentId, studentName, studentSurname, studentLogin, hashedPassword, groupId);
            //  Добавляем студента
            await _studentRepository.AddStudent(student);
            //  Создаем заметки
            await _noteRepository.CreateEmptyNotesForStudent(studentId, groupId);
            transaction.Complete();
        }
        catch
        {
            transaction.Dispose();
            throw;
        }
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