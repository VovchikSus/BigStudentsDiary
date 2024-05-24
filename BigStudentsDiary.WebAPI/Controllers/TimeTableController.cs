using System.Runtime.InteropServices.JavaScript;
using System.Text.RegularExpressions;
using BigStudentsDiary.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace BigStudentsDiary.WebAPI.Controllers;

public class TimeTableController : Controller
{
    ITimeTableRepository timeTableRepository;

    public TimeTableController(ITimeTableRepository repository)
    {
        this.timeTableRepository = repository;
    }

    // GET: api/<TimeTableController>/SearchByGroup without date
    [HttpGet("api/SearchByGroup/{groupId}")]
    public async Task<ActionResult> GetByGroupId(int groupId)
    {
        var result = await this.timeTableRepository.GetTimeTableByGroup(groupId);
        if (result.Result.Any())
            return Ok(result);

        return BadRequest("Расписания нет");
    }

// GET api/<TimeTableController>/SearchByGroup with date
    [HttpGet("api/SearchByGroup/{groupId}/{dayDate}")]
    public async Task<ActionResult> GetByGroupIdAndDate(int groupId,
        [SwaggerParameter("dd-MM-yyyy", Required = true)]DateTime dayDate)
    {
        var result = await this.timeTableRepository.GetTimeTableByGroup(groupId, dayDate.Date);
        if (result.Result.Any())
            return Ok(result);

        return BadRequest("Расписания нет");
    }

// GET: api/<TimeTableController>/SearchByTeacher without date
    [HttpGet("api/SearchByTeacher/{teacherInternalId}")]
    public async Task<ActionResult> GetByTeacherId(int teacherInternalId)
    {
        var result = await this.timeTableRepository.GetTimeTableByTeacher(teacherInternalId);
        if (result.Result.Any())
            return Ok(result);

        return BadRequest("Расписания нет");
    }

// GET api/<TimeTableController>/SearchByTeacher with date
    [HttpGet("api/SearchByTeacher/{teacherInternalId}/{dayDate}")]
    public async Task<ActionResult> GetByTeacherIdAndDate(int teacherInternalId,
        [SwaggerParameter("dd-MM-yyyy", Required = true)]DateTime dayDate)
    {
        var result = await this.timeTableRepository.GetTimeTableByTeacher(teacherInternalId, dayDate.Date);
        if (result.Result.Any())
            return Ok(result);

        return BadRequest("Расписания нет");
    }
}