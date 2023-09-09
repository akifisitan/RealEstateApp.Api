using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RealEstateApp.Api.Auth;
using RealEstateApp.Api.DatabaseContext;
using RealEstateApp.Api.DTO.UserDTO;

namespace RealEstateApp.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private readonly RealEstateContext _context;

        public UserController(RealEstateContext context)
        {
            _context = context;
        }

        [HttpGet]
        [Authorize(Roles = UserRoles.Admin)]
        [Route("list")]
        public async Task<IActionResult> List()
        {
            var result = await _context.Users.AsNoTracking().ToListAsync();
            var users = new List<UserListDTO>();
            foreach (var user in result)
            {
                users.Add(new UserListDTO()
                {
                    Id = user.Id,
                    Username = user.Username,
                    Email = user.Email
                });
            }
            return Ok(users);
        }
    }
}
