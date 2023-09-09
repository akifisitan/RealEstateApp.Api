using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using RealEstateApp.Api.Auth;
using RealEstateApp.Api.DatabaseContext;
using RealEstateApp.Api.DTO.AuthDTO;
using RealEstateApp.Api.Entity;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace RealEstateApp.Api.Controllers
{
    [AllowAnonymous]
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticateController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _configuration;
        private readonly RealEstateContext _context;

        public AuthenticateController(
            UserManager<IdentityUser> userManager,
            RoleManager<IdentityRole> roleManager,
            IConfiguration configuration,
            RealEstateContext context)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _configuration = configuration;
            _context = context;
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDTO request)
        {
            var user = await _userManager.FindByNameAsync(request.Username);
            if (user != null && await _userManager.CheckPasswordAsync(user, request.Password))
            {
                var userRoles = await _userManager.GetRolesAsync(user);

                var student = await _context.Users.SingleAsync(s => s.Email == user.Email);

                var authClaims = new List<Claim>
                {
                    new Claim("Id", student.Id.ToString()),
                    new Claim(ClaimTypes.Name, user.UserName),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                };

                foreach (var userRole in userRoles)
                {
                    authClaims.Add(new Claim(ClaimTypes.Role, userRole));
                }

                var token = GetToken(authClaims);

                return Ok(new
                {
                    token = new JwtSecurityTokenHandler().WriteToken(token),
                    expiration = token.ValidTo,
                    roles = userRoles
                });
            }
            return Unauthorized();
        }

        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequestDTO model)
        {
            if (!model.IsValid())
                return BadRequest(new ResponseDTO { Status = "Error", Message = "User creation failed. Please check user details and try again." });

            var userExists = await _userManager.FindByNameAsync(model.Username);
            if (userExists != null)
                return Conflict(new ResponseDTO { Status = "Error", Message = "Username or email already in use." });

            var emailExists = await _userManager.FindByEmailAsync(model.Email);
            if (emailExists != null)
                return Conflict(new ResponseDTO { Status = "Error", Message = "Username or email already in use." });

            IdentityUser user = new()
            {
                Email = model.Email,
                SecurityStamp = Guid.NewGuid().ToString(),
                UserName = model.Username
            };
            var result = await _userManager.CreateAsync(user, model.Password);
            if (!result.Succeeded)
                return StatusCode(StatusCodes.Status500InternalServerError, new ResponseDTO { Status = "Error", Message = "User creation failed. Please check user details and try again." });

            await _userManager.AddToRoleAsync(user, UserRoles.User);

            User newUser = new()
            {
                Name = user.UserName,
                Email = user.Email,
                Username = user.UserName
            };
            _context.Users.Add(newUser);
            await _context.SaveChangesAsync();

            return Ok(new ResponseDTO { Status = "Success", Message = "User created successfully." });
        }

        [HttpPost]
        [Route("register-admin")]
        public async Task<IActionResult> RegisterAdmin([FromBody] RegisterRequestDTO model)
        {
            if (model.Username == null || model.Email == null || model.Password == null)
                return StatusCode(StatusCodes.Status500InternalServerError, new ResponseDTO { Status = "Error", Message = "User creation failed. Please check user details and try again." });
            model.Username = model.Username.Trim();
            model.Email = model.Email.Trim();
            model.Password = model.Password.Trim();
            var userExists = await _userManager.FindByNameAsync(model.Username);
            if (userExists != null)
                return StatusCode(StatusCodes.Status500InternalServerError, new ResponseDTO { Status = "Error", Message = "User already exists." });

            IdentityUser user = new()
            {
                Email = model.Email,
                SecurityStamp = Guid.NewGuid().ToString(),
                UserName = model.Username
            };
            var result = await _userManager.CreateAsync(user, model.Password);
            if (!result.Succeeded)
                return StatusCode(StatusCodes.Status500InternalServerError, new ResponseDTO { Status = "Error", Message = "User creation failed. Please check user details and try again." });

            if (!await _roleManager.RoleExistsAsync(UserRoles.Admin))
                await _roleManager.CreateAsync(new IdentityRole(UserRoles.Admin));
            if (!await _roleManager.RoleExistsAsync(UserRoles.User))
                await _roleManager.CreateAsync(new IdentityRole(UserRoles.User));

            if (await _roleManager.RoleExistsAsync(UserRoles.Admin))
            {
                await _userManager.AddToRoleAsync(user, UserRoles.Admin);
            }
            if (await _roleManager.RoleExistsAsync(UserRoles.User))
            {
                await _userManager.AddToRoleAsync(user, UserRoles.User);
            }
            User newUser = new()
            {
                Name = user.UserName,
                Email = user.Email,
                Username = user.UserName
            };
            _context.Users.Add(newUser);
            await _context.SaveChangesAsync();

            return Ok(new ResponseDTO { Status = "Success", Message = "User created successfully." });
        }

        [HttpPost]
        [Authorize(Roles = UserRoles.Admin)]
        [Route("create-role")]
        public async Task<IActionResult> CreateRole(string roleName)
        {
            roleName = roleName.Trim();
            if (await _roleManager.RoleExistsAsync(roleName))
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ResponseDTO { Status = "Error", Message = "Role already exists." });
            }
            await _roleManager.CreateAsync(new IdentityRole(roleName));
            return Ok(new ResponseDTO { Status = "Success", Message = "Role created successfully." });
        }

        [HttpDelete]
        [Authorize(Roles = UserRoles.Admin)]
        [Route("delete-role")]
        public async Task<IActionResult> DeleteRole(string roleName)
        {
            roleName = roleName.Trim();
            if (!await _roleManager.RoleExistsAsync(roleName))
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ResponseDTO { Status = "Error", Message = "Role does not exist." });
            }
            await _roleManager.DeleteAsync(new IdentityRole(roleName));
            return NoContent();
        }

        [HttpPost]
        [Authorize(Roles = UserRoles.Admin)]
        [Route("assign-role")]
        public async Task<IActionResult> AssignRole([FromBody] AssignRoleRequestDTO request)
        {
            var user = await _userManager.FindByNameAsync(request.Username);
            if (user == null)
                return StatusCode(StatusCodes.Status500InternalServerError, new ResponseDTO { Status = "Error", Message = "User does not exist." });
            var roleName = request.Role.Trim();
            if (await _roleManager.RoleExistsAsync(roleName))
            {
                await _userManager.AddToRoleAsync(user, roleName);
                return Ok(new ResponseDTO { Status = "Success", Message = "Role assigned successfully." });
            }
            return StatusCode(StatusCodes.Status500InternalServerError, new ResponseDTO { Status = "Error", Message = "Role does not exist." });
        }

        private JwtSecurityToken GetToken(List<Claim> authClaims)
        {
            var secKey = _configuration["JWT:Secret"] ?? throw new Exception("Secret key is null");
            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secKey));
            var token = new JwtSecurityToken(
                issuer: _configuration["JWT:ValidIssuer"],
                audience: _configuration["JWT:ValidAudience"],
                expires: DateTime.UtcNow.AddHours(6),
                claims: authClaims,
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
            );
            return token;
        }
    }
}
