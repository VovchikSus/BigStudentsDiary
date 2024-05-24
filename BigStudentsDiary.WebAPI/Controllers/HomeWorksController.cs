using BigStudentsDiary.Core.Implementations;
using BigStudentsDiary.Domain.Interfaces;
using BigStudentsDiary.Domain.Models;
using Microsoft.AspNetCore.Mvc;

namespace BigStudentsDiary.WebAPI.Controllers;
[Route("api/[controller]")]
[ApiController]
public class HomeWorksController : Controller
{
    IHomeWorksRepository homeWorksRepository;
    
    public HomeWorksController(IHomeWorksRepository repository)
    {
        this.homeWorksRepository = repository;
    }

    // GET: api/<HomeWorksController>
    [HttpGet]
    public async Task<ActionResult> Get() =>
        Ok(await this.homeWorksRepository.GetAllAsync());
    
    // GET api/<HomeWorksController>/5
    [HttpGet("{id}")]
    public async Task<ActionResult> Get(Guid id)
    {
        var result = await this.homeWorksRepository.GetAllAsync(x => x.HomeWorkId == id);
        if (result.Result.Any())
            return Ok(result);

        return BadRequest($"ДЗ с таким id={id} не найден!");
    }

    // POST api/<HomeWorksController>
    [HttpPost]
    public async Task<ActionResult> Post([FromBody] HomeWorks homeWorks)
    {
        if (homeWorks?.HomeWorkDiscription == null)
        {
            return BadRequest("ДЗ не передано или его описание пустое");
        }
        
       
        var result = await this.homeWorksRepository.AddHomeWork(homeWorks);

        return Created($"{this.HttpContext.Request.PathBase}/api/homeworks/{result.Result}", null);
    }

    // PUT api/<HomeWorksController>/5
    [HttpPut("{id}")]
    public async Task<ActionResult> Put(HomeWorks homeWorks)
    {
        if (homeWorks?.HomeWorkDiscription == null)
        {
            return BadRequest("ДЗ не передано или его описание пустое");
        }

        var result = await this.homeWorksRepository.EditHomeWork(homeWorks);
        if (result is ElementNotFound error)
            return BadRequest(error.ErrorMessage);

        return Ok();
    }

    // DELETE api/<HomeWorksController>/5
    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(Guid id)
    {
        var result = await this.homeWorksRepository.DeleteHomeWork(id);
        if (result is ElementNotFound error)
            return BadRequest(error.ErrorMessage);

        return Ok();
    }
}