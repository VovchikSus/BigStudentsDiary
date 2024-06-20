using BigStudentsDiary.Domain.Interfaces.IRepositories;
using Microsoft.AspNetCore.Mvc;

namespace BigStudentsDiary.WebAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class FloorsController : Controller
{
    private readonly IFloorRepository _floorRepository;

    public FloorsController(IFloorRepository repository)
    {
        this._floorRepository = repository;
    }
    
    // GET: api/students
    [HttpGet]
    public async Task<ActionResult> Get()
    {
        var result = await this._floorRepository.GetAllAsync();
        return Ok(result);
    }
    
    // GET api/students/{id}
    [HttpGet("{id:int}")]
    public async Task<ActionResult> GetById(int id)
    {
        var result = await this._floorRepository.GetAllAsync(x => x.FloorId == id);
        if (result.Result.Any())
            return Ok(result);

        return NotFound($"Этаж с id={id} не найден!");
    }
}