using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TimeSheet.Models;
using TimeSheet.Services;

namespace TimeSheet.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TimesheetController : ControllerBase
    {
        private readonly ITimesheetService _service;
        private readonly IUserService _userService;
        private readonly IProjectService _projectService;

        public TimesheetController(ITimesheetService service, IUserService userService,IProjectService projectService)
        {
            _service = service;
            _userService = userService;
            _projectService = projectService;
        }

        // GET all timesheets
        [HttpGet]
        [Authorize(Roles = $"{nameof(Role.ADMIN)}")]
        public IActionResult GetAll() => Ok(_service.GetAll());

        // GET timesheet by id
        [HttpGet("{id:int}")]
        [Authorize(Roles = $"{nameof(Role.ADMIN)}, {nameof(Role.EMPLOYEE)}")]
        public IActionResult GetById(int id)
        {
            var entry = _service.GetById(id);
            return entry is null ? NotFound() : Ok(entry);
        }

        // POST create timesheet
        [HttpPost]
        [Authorize(Roles = $"{nameof(Role.EMPLOYEE)}")]
        public IActionResult Create([FromBody] Timesheet timesheet)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            // Validate user exists
            var user = _userService.GetById(timesheet.UserId);
            if (user == null) return NotFound($"User {timesheet.UserId} not found");

            // Validate project exists
            var project = _projectService.GetById(timesheet.ProjectId);
            if (project == null) return NotFound($"Project {timesheet.ProjectId} not found");

            // Attach related entities
            timesheet.User = user;
            timesheet.Project = project;

            var created = _service.Create(timesheet);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        // PUT update timesheet
        [HttpPut("{id:int}")]
        [Authorize(Roles = $"{nameof(Role.EMPLOYEE)}")]
        public IActionResult Update(int id, [FromBody] Timesheet timesheet)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            if (id != timesheet.Id) return BadRequest("Mismatched id");

            var existing = _service.GetById(id);
            if (existing == null) return NotFound();

            // Validate user exists
            var user = _userService.GetById(timesheet.UserId);
            if (user == null) return NotFound($"User {timesheet.UserId} not found");

            // Validate project exists
            var project = _projectService.GetById(timesheet.ProjectId);
            if (project == null) return NotFound($"Project {timesheet.ProjectId} not found");

            // Update fields
            existing.UserId = timesheet.UserId;
            existing.ProjectId = timesheet.ProjectId;
            existing.Date = timesheet.Date;
            existing.HoursWorked = timesheet.HoursWorked;
            existing.TaskDescription = timesheet.TaskDescription;
            existing.User = user;
            existing.Project = project;

            try
            {
                var updated = _service.Update(existing);
                return Ok(updated);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }

        // DELETE timesheet
        [HttpDelete("{id:int}")]
        [Authorize(Roles = $"{nameof(Role.EMPLOYEE)}")]
        public IActionResult Delete(int id)
        {
            var existing = _service.GetById(id);
            if (existing == null) return NotFound();

            _service.Delete(id);
            return NoContent();
        }
    }
}
