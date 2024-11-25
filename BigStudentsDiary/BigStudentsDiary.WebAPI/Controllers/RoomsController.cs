using BigStudentsDiary.Domain.Interfaces.IRepositories;
using Microsoft.AspNetCore.Mvc;

namespace BigStudentsDiary.WebAPI.Controllers;


[Route("api/[controller]")]
[ApiController]
public class RoomsController: Controller
{
    private readonly IRoomRepository _roomRepository;

    public RoomsController(IRoomRepository repository)
    {
        this._roomRepository = repository;
    }
    
    // GET: api/rooms
    [HttpGet]
    public async Task<ActionResult> Get()
    {
        var result = await this._roomRepository.GetAllAsync();
        return Ok(result);
    }
    
    // GET api/rooms/{id}
    [HttpGet("{id:int}")]
    public async Task<ActionResult> GetById(int id)
    {
        var result = await this._roomRepository.GetAllAsync(x => x.RoomId == id);
        if (result.Result.Any())
            return Ok(result);

        return NotFound($"кабинет с id={id} не найден!");
    }
}