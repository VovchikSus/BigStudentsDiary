using BigStudentsDiary.Domain.Interfaces.IRepositories;
using Microsoft.AspNetCore.Mvc;

namespace BigStudentsDiary.WebAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class BuildingController : Controller
{
    private readonly IBuildingRepository _buildingRepository;

    public BuildingController(IBuildingRepository repository)
    {
        this._buildingRepository = repository;
    }

    // GET: api/building
    [HttpGet]
    public async Task<ActionResult> Get()
    {
        var result = await this._buildingRepository.GetAllAsync();
        return Ok(result);
    }

    // GET api/buildings/{id}
    [HttpGet("{id:int}")]
    public async Task<ActionResult> GetById(int id)
    {
        var result = await this._buildingRepository.GetAllAsync(x => x.BuildingId == id);
        if (result.Result.Any())
            return Ok(result);

        return NotFound($"Stroenie с id={id} не найден!");
    }
}