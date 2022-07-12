using System.Web.Http;
using Microsoft.AspNetCore.Mvc;
using RouteAttribute = Microsoft.AspNetCore.Mvc.RouteAttribute;
using HttpGetAttribute = Microsoft.AspNetCore.Mvc.HttpGetAttribute;
using HttpPostAttribute = Microsoft.AspNetCore.Mvc.HttpPostAttribute;
using HttpPatchAttribute = Microsoft.AspNetCore.Mvc.HttpPatchAttribute;
using HttpDeleteAttribute = Microsoft.AspNetCore.Mvc.HttpDeleteAttribute;
using FromBodyAttribute = Microsoft.AspNetCore.Mvc.FromBodyAttribute;
using server.Models;
using server.Services;

namespace server.Controllers
{
    [ApiController]
    [Route("api/users")]
    public class UserController : ControllerBase    
    {
        private readonly ILogger<UserController> _logger;
        private readonly IUserService _userService;

        public UserController(ILogger<UserController> logger,
            IUserService userService
            )
        {
            _logger = logger;
            _userService = userService;
        }

        [HttpGet("search")]
        public ActionResult<IEnumerable<User>> GetUsers([FromQuery] string firstName, [FromQuery] string lastName, [FromQuery] string email)
        {
            var users = _userService.GetUsers(firstName, lastName, email);
            if (users == null)
            {
                return NotFound();
            }
            return Ok(users);
        }

        [HttpGet("{userId}")]
        public ActionResult<User> GetUser([FromRoute] int userId)
        {
            // TODO: Ideally we should not be exposing db id's.
            // A better option would be to have public keys (GUID) for each of the entities
            // and query data based on those public keys.
            var user = _userService.GetUser(userId);
            if (user == null)
            {
                return NotFound();
            }
            return Ok(user);
        }

        [HttpPost]
        public ActionResult<bool> AddUser([FromBody] User user)
        {
            var addUserSuccessful = _userService.AddUser(user);
            if (addUserSuccessful)
            {
                return Ok();
            }
            return NoContent();
        }

        [HttpPatch]
        public ActionResult<bool> UpdateUser([FromBody] User user)
        {
            var updateUserSuccessful = _userService.UpdateUser(user);
            if (updateUserSuccessful)
            {
                return Ok();
            }
            return NotFound();
        }

        [HttpDelete("{userId}")]
        public IActionResult DeleteUser([FromRoute] int userId)
        {
            // TODO: Ideally we should not be exposing db id's.
            // A better option would be to have public keys (GUID) for each of the entities
            // and query/process data based on those public keys.
            var deleteSuccessfull = _userService.DeleteUser(userId);

            if (deleteSuccessfull)
            {
                return NoContent();
            }
            return NotFound();
        }
    }
}
