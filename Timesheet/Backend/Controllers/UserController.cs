using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TimeSheet.Models;
using TimeSheet.Services;

namespace TimeSheet.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _service;
        public UserController(IUserService service) => _service = service;

        [HttpGet]
        [Authorize(Roles = $"{nameof(Role.ADMIN)}")]
        public IActionResult GetAll() => Ok(_service.GetAll());

        [HttpGet("{id:int}")]
        [Authorize(Roles = $"{nameof(Role.ADMIN)}, {nameof(Role.EMPLOYEE)}")]
        public IActionResult GetById(int id)
        {
            var user = _service.GetById(id);
            return user is null ? NotFound() : Ok(user);
        }

        [HttpPost("register")]
        [Authorize(Roles = $"{nameof(Role.ADMIN)}, {nameof(Role.EMPLOYEE)}")]
        public IActionResult Post([FromBody] User user)
        {
            if (ModelState.IsValid)
            {
                var addedUser = _service.Add(user);
                return CreatedAtAction(nameof(GetById), new { id = addedUser.UserId }, addedUser);
            }
            return BadRequest(user);
        }

        [HttpPut("{id:int}")]
        [Authorize(Roles = $"{nameof(Role.ADMIN)}, {nameof(Role.EMPLOYEE)}")]
        public IActionResult Update(int id, [FromBody] User user)
        {
            if (id != user.UserId) return BadRequest("Mismatched id");
            if (!ModelState.IsValid) return BadRequest(ModelState);

            try
            {
                var updated = _service.Update(user);
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
            var existing = _service.GetById(id);
            if (existing == null) return NotFound();

            _service.Delete(id);
            return NoContent();
        }
    }
}
