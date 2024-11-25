using BigStudentsDiary.Core.Implementations;
using BigStudentsDiary.Domain.Interfaces;
using BigStudentsDiary.Domain.Interfaces.IRepositories;
using BigStudentsDiary.Domain.Models;
using Microsoft.AspNetCore.Mvc;

namespace BigStudentsDiary.WebAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class StudentsController : Controller
{
    private readonly IStudentsRepository _studentsRepository;
    
    public StudentsController(IStudentsRepository repository)
    {
        this._studentsRepository = repository;
    }

    // GET: api/students
    [HttpGet]
    public async Task<ActionResult> Get()
    {
        var result = await this._studentsRepository.GetAllAsync();
        return Ok(result);
    }
    
    // GET api/students/{id}
    [HttpGet("{id:guid}")]
    public async Task<ActionResult> GetById(Guid id)
    {
        var result = await this._studentsRepository.GetAllAsync(x => x.StudentId == id);
        if (result.Result.Any())
            return Ok(result);

        return NotFound($"Студент с id={id} не найден!");
    }
    
    // [HttpGet("group")]
    // public IActionResult GetGroup()
    // {
    //     var groupIdClaim = User.Claims.FirstOrDefault(c => c.Type == "groupId");
    //     if (groupIdClaim == null)
    //     {
    //         return Unauthorized("GroupId not found in token");
    //     }
    //
    //     var groupId = int.Parse(groupIdClaim.Value);
    //     // Используйте groupId для выполнения запроса
    //     var group = studentsRepository.GetGroupById(groupId);
    //     return Ok(group);
    // }
    
    // GET: api/students/login/{login}
    [HttpGet("login/{login}")]
    public async Task<ActionResult> GetByLogin(string login)
    {
        var result = await this._studentsRepository.GetAllAsync(x => x.StudentLogin == login);
        if (result.Result.Any())
            return Ok(result);

        return NotFound($"Студент с login={login} не найден!");
    }

    // POST api/students
    [HttpPost]
    public async Task<ActionResult> Post([FromBody] Students student)
    {
        if (student?.Name == null)
        {
            return BadRequest("Студент не передан или его имя пустое");
        }
        
        if (student?.Surname == null)
        {
            return BadRequest("Студент не передан или его фамилия пуста");
        }
        
        if (student?.StudentLogin == null)
        {
            return BadRequest("Студент не передан или его логин пуст");
        }
        
        if (student?.StudentPassword == null)
        {
            return BadRequest("Студент не передан или его пароль пуст");
        }
        
        if (student?.GroupId == null)
        {
            return BadRequest("Студент не передан или его группа пустая");
        }

        var result = await this._studentsRepository.AddStudent(student);

        return Created($"{this.HttpContext.Request.PathBase}/api/students/{result.Result}", null);
    }

    // PUT api/students/{id}
    [HttpPut("{id:guid}")]
    public async Task<ActionResult> Put(Guid id, [FromBody] Students student)
    {
        if (student?.Name == null)
        {
            return BadRequest("Студент не передан или его имя пустое");
        }

        student.StudentId = id;
        var result = await this._studentsRepository.EditStudent(student);
        if (result is ElementNotFound error)
            return BadRequest(error.ErrorMessage);

        return Ok();
    }

    // DELETE api/students/{id}
    [HttpDelete("{id:guid}")]
    public async Task<ActionResult> Delete(Guid id)
    {
        var result = await this._studentsRepository.DeleteStudent(id);
        if (result is ElementNotFound error)
            return BadRequest(error.ErrorMessage);

        return Ok();
    }
}
