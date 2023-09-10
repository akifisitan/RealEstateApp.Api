using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using RealEstateApp.Api.Auth;
using RealEstateApp.Api.DatabaseContext;
using RealEstateApp.Api.DTO;
using RealEstateApp.Api.DTO.PropertyFieldDTO;
using RealEstateApp.Api.Entity;
using RealEstateApp.Api.Enums;

namespace RealEstateApp.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CurrencyController : ControllerBase
    {
        private readonly RealEstateContext _context;
        private readonly DbSet<Currency> _set;

        public CurrencyController(RealEstateContext context)
        {
            _context = context;
            _set = _context.Currencies;
        }

        [HttpGet]
        [Route("list")]
        public async Task<IActionResult> List()
        {
            var result = await _set
                .AsNoTracking()
                .Where(p => p.Status != (int)EntityStatus.Deleted)
                .ToListAsync();

            if (result == null) return NotFound();

            var infoList = new List<PropertyFieldInfoDTO<Currency>>();
            result.ForEach(x => infoList.Add(new PropertyFieldInfoDTO<Currency>(x)));

            return Ok(infoList);
        }

        [Authorize(Roles = UserRoles.Admin)]
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] PropertyFieldCreateRequestDTO request)
        {
            var value = request.Value.Trim();

            if (value.IsNullOrEmpty()) return BadRequest(new GenericResponse<string>(null, "Please enter a non-empty value"));

            var caseInsensitiveValue = value.ToLower();

            var existingItem = await _set
                .SingleOrDefaultAsync(x => x.Value.ToLower() == caseInsensitiveValue &&
                                           x.Status != (int)EntityStatus.Deleted);

            if (existingItem != null) return Conflict(new GenericResponse<string>(null, "Please enter a unique value"));

            var item = _set.Add(new Currency(value));
            await _context.SaveChangesAsync();

            return Ok(new PropertyFieldInfoDTO<Currency>(item.Entity));
        }

        [Authorize(Roles = UserRoles.Admin)]
        [HttpPut]
        public async Task<IActionResult> Put([FromBody] PropertyFieldUpdateRequestDTO request)
        {
            var value = request.Value.Trim();

            if (value.IsNullOrEmpty()) return BadRequest(new GenericResponse<string>(null, "Please enter a non-empty value"));

            var item = await _set
                .SingleOrDefaultAsync(x => x.Id == request.Id &&
                                           x.Status != (int)EntityStatus.Deleted);

            if (item == null) return NotFound();

            var caseInsensitiveValue = value.ToLower();

            var existingItem = await _set
                .SingleOrDefaultAsync(x => x.Value.ToLower() == caseInsensitiveValue &&
                                           x.Status != (int)EntityStatus.Deleted);

            if (existingItem != null) return Conflict(new GenericResponse<string>(null, "Please enter a unique value"));

            item.Value = value;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [Authorize(Roles = UserRoles.Admin)]
        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            var item = await _set.SingleOrDefaultAsync(x => x.Id == id && x.Status != (int)EntityStatus.Deleted);

            if (item == null) return NotFound();

            item.Status = (int)EntityStatus.Deleted;
            await _context.SaveChangesAsync();

            return NoContent();
        }

    }
}
