using BigStudentsDiary.Core.Implementations;
using BigStudentsDiary.Domain.Interfaces;
using BigStudentsDiary.Domain.Models;
using Microsoft.AspNetCore.Mvc;

namespace BigStudentsDiary.WebAPI.Controllers;
[Route("api/[controller]")]
[ApiController]
public class HomeWorksController : Controller
{
    readonly IHomeWorksRepository _homeWorksRepository;
    
    public HomeWorksController(IHomeWorksRepository repository)
    {
        this._homeWorksRepository = repository;
    }

    // GET: api/<HomeWorksController>
    [HttpGet]
    public async Task<ActionResult> Get() =>
        Ok(await this._homeWorksRepository.GetAllAsync());
    
    // GET api/<HomeWorksController>/5
    [HttpGet("{id}")]
    public async Task<ActionResult> Get(Guid id)
    {
        var result = await this._homeWorksRepository.GetAllAsync(x => x.HomeWorkId == id);
        if (result.Result.Any())
            return Ok(result);

        return BadRequest($"ДЗ с таким id={id} не найден!");
    }

    // POST api/<HomeWorksController>
    [HttpPost]
    public async Task<ActionResult> Post([FromBody] HomeWorks homeWorks)
    {
        if (homeWorks?.HomeWorkDescription == null)
        {
            return BadRequest("ДЗ не передано или его описание пустое");
        }
        
       
        var result = await this._homeWorksRepository.AddHomeWork(homeWorks);

        return Created($"{this.HttpContext.Request.PathBase}/api/homeworks/{result.Result}", null);
    }

    // PUT api/<HomeWorksController>/5
    [HttpPut("{id}")]
    public async Task<ActionResult> Put(HomeWorks homeWorks)
    {
        if (homeWorks?.HomeWorkDescription == null)
        {
            return BadRequest("ДЗ не передано или его описание пустое");
        }

        var result = await this._homeWorksRepository.EditHomeWork(homeWorks);
        if (result is ElementNotFound error)
            return BadRequest(error.ErrorMessage);

        return Ok();
    }

    // DELETE api/<HomeWorksController>/5
    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(Guid id)
    {
        var result = await this._homeWorksRepository.DeleteHomeWork(id);
        if (result is ElementNotFound error)
            return BadRequest(error.ErrorMessage);

        return Ok();
    }
}