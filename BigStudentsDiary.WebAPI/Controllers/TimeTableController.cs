using System.Runtime.InteropServices.JavaScript;
using System.Text.RegularExpressions;
using BigStudentsDiary.Domain.Interfaces;
using BigStudentsDiary.Domain.Models;
using BigStudentsDiary.Domain.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace BigStudentsDiary.WebAPI.Controllers;

public class TimeTableController : Controller
{
    ITimeTableRepository timeTableRepository;
    private readonly TimeTableService _timeTableService;
    public TimeTableController(ITimeTableRepository repository, TimeTableService timeTableService)
    {
        this.timeTableRepository = repository;
        _timeTableService = timeTableService;
    }

    
    
    [HttpGet("/date/{dayDate}")]
    [Authorize]
    public async Task<ActionResult<IEnumerable<FullTimeTableDto>>> GetFullTimeTableByGroup(
        DateTime dayDate)
    {
        var groupIdClaim = User.Claims.FirstOrDefault(c => c.Type == "groupId");
        if (groupIdClaim == null || !int.TryParse(groupIdClaim.Value, out var groupId))
        {
            return Unauthorized();
        }
        var fullTimeTable = await _timeTableService.GetFullTimeTableByGroup(groupId, dayDate);
        return Ok(fullTimeTable);
    }
    
    
    
    [HttpGet("/GetTimeTableToday")]
    [Authorize]
    public async Task<ActionResult<IEnumerable<FullTimeTableDto>>> GetFullTimeTableByGroup()
    {
        var groupIdClaim = User.Claims.FirstOrDefault(c => c.Type == "groupId");
        if (groupIdClaim == null || !int.TryParse(groupIdClaim.Value, out var groupId))
        {
            return Unauthorized();
        }
        var fullTimeTable = await _timeTableService.GetFullTimeTableByGroup(groupId);
        return Ok(fullTimeTable);
    }

    [HttpGet("GetTeacherTimeTable/date/{dayDate}")]
    [Authorize]
    public async Task<ActionResult<IEnumerable<FullTimeTableDto>>> GetFullTimeTableByTeacher(DateTime dayDate)
    {
        var teacherIdClaim = User.Claims.FirstOrDefault(c => c.Type == "teacherId");
        if (teacherIdClaim == null || !int.TryParse(teacherIdClaim.Value, out var teacherId))
        {
            return Unauthorized();
        }
        var fullTimeTable = await _timeTableService.GetFullTimeTableByTeacher(teacherId,dayDate);
        return Ok(fullTimeTable);
    }
    
    [HttpGet("/GetTeacherTimeTableToday")]
    [Authorize]
    public async Task<ActionResult<IEnumerable<FullTimeTableDto>>> GetFullTimeTableByTeacher()
    {
        var teacherIdClaim = User.Claims.FirstOrDefault(c => c.Type == "teacherId");
        if (teacherIdClaim == null || !int.TryParse(teacherIdClaim.Value, out var teacherId))
        {
            return Unauthorized();
        }
        var fullTimeTable = await _timeTableService.GetFullTimeTableByTeacher(teacherId);
        return Ok(fullTimeTable);
    }
    
    
//     // GET: api/<TimeTableController>/SearchByGroup without date
//     [HttpGet("api/SearchByGroup/{groupId}")]
//     public async Task<ActionResult> GetByGroupId(int groupId)
//     {
//         var result = await this.timeTableRepository.GetTimeTableByGroup(groupId);
//         if (result.Result.Any())
//             return Ok(result);
//
//         return BadRequest("Расписания нет");
//     }
//
// // GET api/<TimeTableController>/SearchByGroup with date
//     [HttpGet("api/SearchByGroup/{groupId}/{dayDate}")]
//     public async Task<ActionResult> GetByGroupIdAndDate(int groupId,
//         [SwaggerParameter("dd-MM-yyyy", Required = true)]
//         DateTime dayDate)
//     {
//         var result = await this.timeTableRepository.GetTimeTableByGroup(groupId, dayDate.Date);
//         if (result.Result.Any())
//             return Ok(result);
//
//         return BadRequest("Расписания нет");
//     }
//
// // GET: api/<TimeTableController>/SearchByTeacher without date
//     [HttpGet("api/SearchByTeacher/{teacherInternalId}")]
//     public async Task<ActionResult> GetByTeacherId(int teacherInternalId)
//     {
//         var result = await this.timeTableRepository.GetTimeTableByTeacher(teacherInternalId);
//         if (result.Result.Any())
//             return Ok(result);
//
//         return BadRequest("Расписания нет");
//     }
//
// // GET api/<TimeTableController>/SearchByTeacher with date
//     [HttpGet("api/SearchByTeacher/{teacherInternalId}/{dayDate}")]
//     public async Task<ActionResult> GetByTeacherIdAndDate(int teacherInternalId,
//         [SwaggerParameter("dd-MM-yyyy", Required = true)]
//         DateTime dayDate)
//     {
//         var result = await this.timeTableRepository.GetTimeTableByTeacher(teacherInternalId, dayDate.Date);
//         if (result.Result.Any())
//             return Ok(result);
//
//         return BadRequest("Расписания нет");
//     }
}