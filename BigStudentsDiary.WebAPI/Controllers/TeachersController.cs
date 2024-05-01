using BigStudentsDiary.Core.Implementations;
using BigStudentsDiary.Domain.Interfaces;
using BigStudentsDiary.Domain.Models;
using Microsoft.AspNetCore.Mvc;

namespace BigStudentsDiary.WebAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class TeachersController : Controller
{
    private ITeachersRepository teachersRepository;


    public TeachersController(ITeachersRepository repository)
    {
        this.teachersRepository = repository;
    }


    //GET: api/<TeacherController>
    [HttpGet]
    public async Task<ActionResult> Get() => Ok(await this.teachersRepository.GetAllAsync());

    //GET api/<TeacherController>/5
    [HttpGet("{id}")]
    public async Task<ActionResult> Get(Guid id)
    {
        var result = await this.teachersRepository.GetAllAsync(x => x.TeacherExternalId == id);
        if (result.Result.Any())
        {
            return Ok(result);
        }

        return BadRequest($"Преподаватель с таким id={id} не найден");
    }


    // POST api/<TeacherController>
    [HttpPost]
    public async Task<ActionResult> Post([FromBody] Teachers teacher)
    {
        if (teacher?.TeacherFio == null)
        {
            return BadRequest("Преподаватель не передан или его имя пустое");
        }

        var result = await this.teachersRepository.AddTeacher(teacher);
        return Created($"{this.HttpContext.Request.PathBase}/api/teachers/{result.Result}", null);
    }

    // PUT api/<TeachersController>/5
    [HttpPut("{id}")]
    public async Task<ActionResult> Put(Teachers teacher)
    {
        if (teacher?.TeacherFio == null)
        {
            return BadRequest("Преподаватель не передан или его имя пустое");
        }

        var result = await this.teachersRepository.EditTeacher(teacher);
        if (result is ElementNotFound error)
            return BadRequest(error.ErrorMessage);

        return Ok();
    }

    // DELETE api/<TeacherController>/5
    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(Guid id)
    {
        var result = await this.teachersRepository.DeleteTeacher(id);
        if (result is ElementNotFound error)
        {
            return BadRequest(error.ErrorMessage);
        }

        return Ok();
    }
}