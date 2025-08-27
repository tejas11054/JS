using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TimeSheet.Models;
using TimeSheet.Services;

namespace TimeSheet.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IUserService _userService;
        public AuthController(IUserService userService)
        {
            _userService = userService;
        }
        [HttpPost]
        [Route("Login")]
        public IActionResult PostLogin([FromBody] LoginViewModel usr)
        {
            if (ModelState.IsValid)
            {
                var response = _userService.Login(usr);
                if (response.isSucess)
                {
                    return Ok(response);
                }
                return Unauthorized();
            }
            return BadRequest();
        }
    }
}
