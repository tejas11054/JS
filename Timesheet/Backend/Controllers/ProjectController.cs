using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TimeSheet.Models;
using TimeSheet.Services;

namespace TimeSheet.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProjectController : ControllerBase
    {
        private readonly IProjectService _service;
        public ProjectController(IProjectService service) => _service = service;

        [HttpGet]
        [Authorize(Roles = $"{nameof(Role.ADMIN)}")]
        public IActionResult GetAll() => Ok(_service.GetAll());

        [HttpGet("{id:int}")]
        [Authorize(Roles = $"{nameof(Role.ADMIN)}, {nameof(Role.EMPLOYEE)}")]
        public IActionResult GetById(int id)
        {
            var proj = _service.GetById(id);
            return proj is null ? NotFound() : Ok(proj);
        }

        [HttpPost]
        [Authorize(Roles = $"{nameof(Role.ADMIN)}")]
        public IActionResult Create([FromBody] Project project)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var created = _service.Create(project);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        [HttpPut("{id:int}")]
        [Authorize(Roles = $"{nameof(Role.ADMIN)}")]
        public IActionResult Update(int id, [FromBody] Project project)
        {
            if (id != project.Id) return BadRequest("Mismatched id");
            if (!ModelState.IsValid) return BadRequest(ModelState);

            try
            {
                var updated = _service.Update(project);
                return Ok(updated);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }

        [HttpDelete("{id:int}")]
        [Authorize(Roles = $"{nameof(Role.ADMIN)}")]
        public IActionResult Delete(int id)
        {
            _service.Delete(id);
            return NoContent();
        }
    }
}
