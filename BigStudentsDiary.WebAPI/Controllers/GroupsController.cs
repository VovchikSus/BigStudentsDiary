using System.Web;
using BigStudentsDiary.Core.Implementations;
using BigStudentsDiary.Domain.Interfaces;
using BigStudentsDiary.Domain.Models;

using Microsoft.AspNetCore.Mvc;

namespace BigStudentsDiary.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GroupsController : Controller
    {
        readonly IGroupRepository _groupRepository;

        public GroupsController(IGroupRepository repository)
        {
            this._groupRepository = repository;
        }

        // GET: api/<GroupsController>
        [HttpGet]
        public async Task<ActionResult> Get() =>
            Ok(await this._groupRepository.GetAllAsync());

        // GET api/<GroupsController>/5
        [HttpGet("{id}")]
        public async Task<ActionResult> Get(int id)
        {
            var result = await this._groupRepository.GetAllAsync(x => x.GroupId == id);
            if (result.Result.Any())
                return Ok(result);

            return BadRequest($"Группа с таким id={id} не найдена!");
        }

        // GET api/<GroupsController>/code/ABC123
        [HttpGet("code/{groupCode}")]
        public async Task<ActionResult> GetByCode(string groupCode)
        {
            var decodedGroupCode = HttpUtility.UrlDecode(groupCode);
            Console.WriteLine($"Decoded GroupCode: {decodedGroupCode}"); // Логирование

            var result = await this._groupRepository.GetByGroupCodeAsync(decodedGroupCode);

            if (result is ElementNotFound<Groups> error)
                return NotFound(error.ErrorMessage);

            return Ok(result.Result);
        }

        // POST api/<GroupsController>
        [HttpPost]
        public async Task<ActionResult> Post([FromBody] Groups group)
        {
            if (group?.GroupCode == null)
            {
                return BadRequest("Группа не передана или ее код пустой");
            }

            var result = await this._groupRepository.AddGroup(group);

            return Created($"{this.HttpContext.Request.PathBase}/api/groups/{result.Result}", null);
        }

        // PUT api/<GroupsController>/5
        [HttpPut("{id}")]
        public async Task<ActionResult> Put(Groups group)
        {
            if (group?.GroupCode == null)
            {
                return BadRequest("Группа не передана или ее код пустой");
            }

            var result = await this._groupRepository.EditGroup(group);
            if (result is ElementNotFound error)
                return BadRequest(error.ErrorMessage);

            return Ok();
        }

        // DELETE api/<GroupsController>/5
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var result = await this._groupRepository.DeleteGroup(id);
            if (result is ElementNotFound error)
                return BadRequest(error.ErrorMessage);

            return Ok();
        }
    }
}
