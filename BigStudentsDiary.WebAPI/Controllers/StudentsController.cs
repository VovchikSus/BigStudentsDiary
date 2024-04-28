using BigStudentsDiary.Core.Implementations;
using BigStudentsDiary.Domain.Interfaces;
using BigStudentsDiary.Domain.Models;
using Microsoft.AspNetCore.Mvc;

namespace BigStudentsDiary.WebAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class StudentsController : Controller
{
    IStudentsRepository studentsRepository;
    
    public StudentsController(IStudentsRepository repository)
    {
        this.studentsRepository = repository;
    }

    // GET: api/<StudentsController>
    [HttpGet]
    public async Task<ActionResult> Get() =>
        Ok(await this.studentsRepository.GetAllAsync());
    
    // GET api/<StudentsController>/5
    [HttpGet("{id}")]
    public async Task<ActionResult> Get(Guid id)
    {
        var result = await this.studentsRepository.GetAllAsync(x => x.StudentId == id);
        if (result.Result.Any())
            return Ok(result);

        return BadRequest($"Студент с таким id={id} не найден!");
    }

    // POST api/<StudentsController>
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

        var result = await this.studentsRepository.AddStudent(student);

        return Created($"{this.HttpContext.Request.PathBase}/api/students/{result.Result}", null);
    }

    // PUT api/<StudentsController>/5
    [HttpPut("{id}")]
    public async Task<ActionResult> Put(Students student)
    {
        if (student?.Name == null)
        {
            return BadRequest("Студент не передан или его имя пустое");
        }

        var result = await this.studentsRepository.EditStudent(student);
        if (result is ElementNotFound error)
            return BadRequest(error.ErrorMessage);

        return Ok();
    }

    // DELETE api/<StudentsController>/5
    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(Guid id)
    {
        var result = await this.studentsRepository.DeleteStudent(id);
        if (result is ElementNotFound error)
            return BadRequest(error.ErrorMessage);

        return Ok();
    }
}