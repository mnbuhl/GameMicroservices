using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Play.Identity.Service.Entities;

namespace Play.Identity.Service.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;

        public UsersController(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }

        // GET /api/users
        [HttpGet]
        public ActionResult<IEnumerable<UserDto>> GetUsers()
        {
            IEnumerable<UserDto> users = _userManager.Users.ToList().Select(user => user.AsDto());
            return Ok(users);
        }

        // GET /api/users/{id}
        [HttpGet("{id:guid}")]
        public async Task<ActionResult<UserDto>> GetById(Guid id)
        {
            var user = await _userManager.FindByIdAsync(id.ToString());

            if (user == null)
            {
                return NotFound();
            }

            return Ok(user.AsDto());
        }

        // PUT /api/users/{id}
        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Update(Guid id, UpdateUserDto userDto)
        {
            var user = await _userManager.FindByIdAsync(id.ToString());

            if (user == null)
            {
                return NotFound();
            }

            user.Email = userDto.Email ?? user.Email;
            user.UserName = userDto.Email ?? user.Email;
            user.Balance = userDto.Balance ?? user.Balance;

            await _userManager.UpdateAsync(user);

            return NoContent();
        }

        // DELETE /api/users/{id}
        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var user = await _userManager.FindByIdAsync(id.ToString());

            if (user == null)
            {
                return NotFound();
            }

            await _userManager.DeleteAsync(user);

            return NoContent();
        }
    }
}