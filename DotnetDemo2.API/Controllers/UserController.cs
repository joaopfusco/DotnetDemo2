using DotnetDemo2.Domain.Models;
using DotnetDemo2.Service.Interfaces;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace DotnetDemo2.API.Controllers
{
    public class UserController(IUserService service, ILogger<UserController> logger) : CrudController<User>(service, logger)
    {
        public override Task<IActionResult> Post([FromBody] User model)
        {
            return Task.FromResult<IActionResult>(BadRequest("Ação não permitida."));
        }

        public override Task<IActionResult> Put(int id, [FromBody] User model)
        {
            return Task.FromResult<IActionResult>(BadRequest("Ação não permitida."));
        }

        public override IActionResult Patch(int id, [FromBody] JsonPatchDocument<User> patchDoc)
        {
            return BadRequest("Ação não permitida.");
        }

        [HttpGet("[action]")]
        public IActionResult Me()
        {
            return TryExecute(() =>
            {
                var claims = User.Claims.ToDictionary(c => c.Type, c => c.Value);
                return Ok(claims);
            });
        }
    }
}
