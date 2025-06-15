using BigStudentsDiary.Domain.Interfaces.IRepositories;
using Microsoft.AspNetCore.Mvc;

namespace BigStudentsDiary.WebAPI.Controllers;

[Route("api/[controller]")]
[ApiController]

public class DisciplineController:Controller
{
    private readonly IDisciplinesRepository _disciplinesRepository;

    public DisciplineController(IDisciplinesRepository repository)
    {
        this._disciplinesRepository = repository;
    }
    // GET: api/disciplines
    [HttpGet]
    public async Task<ActionResult> Get()
    {
        var result = await this._disciplinesRepository.GetAllAsync();
        return Ok(result);
    }
    // GET api/disciplines/{id}
    [HttpGet("{id:int}")]
    public async Task<ActionResult> GetById(int id)
    {
        var result = await this._disciplinesRepository.GetAllAsync(x => x.DisciplineId == id);
        if (result.Result.Any())
            return Ok(result);
        return NotFound($"Дисциплина с id={id} не найдена!");
    }
    
   
    [HttpGet("name/{disciplineName}")]
    public async Task<ActionResult> GetByName(string disciplineName)
    {
        var discipline = await _disciplinesRepository.GetDisciplineIdByName(disciplineName);
        return discipline != null 
            ? Ok(discipline) 
            : NotFound($"Дисциплина '{disciplineName}' не найдена");
    }


    [HttpGet("group/{groupId}")]
    public async Task<ActionResult> GetByGroup(int groupId)
    {
        var result = await _disciplinesRepository.GetDisciplinesByGroup(groupId);
        return result.Successful 
            ? Ok(result.Result) 
            : BadRequest(result.ErrorMessage);
    }
}