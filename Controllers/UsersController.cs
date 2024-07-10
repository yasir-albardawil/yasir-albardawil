using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PlateWebAPI.Controllers
{
    //[Authorize(Roles = "SuperAdmin")]
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public UsersController(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<IdentityUser>>> List(string id, string search, string sortOrder)
        {
            IEnumerable<IdentityUser> users;

            if (string.IsNullOrEmpty(id))
            {
                users = _userManager.Users;
            }
            else
            {
                users = _userManager.Users.Where(u => u.Id == id);
            }

            if (!string.IsNullOrEmpty(search))
            {
                users = users.Where(u => u.UserName!.Contains(search));
            }

            users = sortOrder switch
            {
                "name_desc" => users.OrderByDescending(u => u.UserName),
                "id_desc" => users.OrderByDescending(u => u.Id),
                _ => users.OrderBy(u => u.UserName)
            };

            var roles = await _roleManager.Roles.ToListAsync();
            return Ok(users);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<IdentityUser>> Details(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return NotFound();
            }

            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            return Ok(user);
        }
    }
}
