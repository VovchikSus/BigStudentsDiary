using BigStudentsDiary.Domain.Interfaces.IRepositories;
using Microsoft.AspNetCore.Mvc;

namespace BigStudentsDiary.WebAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class DepartmentsController : Controller
{
    private readonly IDepartmentRepository _departmentRepository;

    public DepartmentsController(IDepartmentRepository repository)
    {
        this._departmentRepository = repository;
    }

    // GET: api/departments
    [HttpGet]
    public async Task<ActionResult> Get()
    {
        var result = await this._departmentRepository.GetAllAsync();
        return Ok(result);
    }

    // GET api/departments/{id}
    [HttpGet("{id:int}")]
    public async Task<ActionResult> GetById(int id)
    {
        var result = await this._departmentRepository.GetAllAsync(x => x.DepartmentId == id);
        if (result.Result.Any())
            return Ok(result);

        return NotFound($"Департамент с id={id} не найден!");
    }
}