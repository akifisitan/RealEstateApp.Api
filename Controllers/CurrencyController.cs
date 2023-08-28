using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using RealEstateApp.Api.Auth;
using RealEstateApp.Api.DatabaseContext;
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
        public async Task<IActionResult> GetAll()
        {
            var result = await _set
                .AsNoTracking()
                .Where(p => p.Status != (int)EntityStatus.Deleted)
                .ToListAsync();

            if (result == null)
                return NotFound();

            var currencyInfoList = new List<PropertyFieldInfoDTO<Currency>>();
            result.ForEach(currency => currencyInfoList.Add(new PropertyFieldInfoDTO<Currency>(currency)));

            return Ok(currencyInfoList);
        }

        [Authorize(Roles = UserRoles.Admin)]
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] PropertyFieldCreateRequestDTO request)
        {
            request.Value = request.Value.Trim();
            if (request.Value.IsNullOrEmpty())
            {
                return BadRequest();
            }
            var item = _set.Add(new Currency(request.Value));
            await _context.SaveChangesAsync();

            return Ok(new PropertyFieldInfoDTO<Currency>(item.Entity));
        }

        [Authorize(Roles = UserRoles.Admin)]
        [HttpPut]
        public async Task<IActionResult> Put([FromBody] PropertyFieldUpdateRequestDTO request)
        {
            request.Value = request.Value.Trim();
            if (request.Value.IsNullOrEmpty())
            {
                return BadRequest();
            }
            var item = await _set
                .SingleOrDefaultAsync(x => x.Id == request.Id && x.Status != (int)EntityStatus.Deleted);
            if (item != null)
            {
                item.Value = request.Value;
                await _context.SaveChangesAsync();
                return Ok(new PropertyFieldInfoDTO<Currency>(item));
            }
            return NotFound();
        }

        [Authorize(Roles = UserRoles.Admin)]
        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            var item = await _set
                .SingleOrDefaultAsync(x => x.Id == id && x.Status != (int)EntityStatus.Deleted);
            if (item != null)
            {
                item.Status = (int)EntityStatus.Deleted;
                await _context.SaveChangesAsync();
                return NoContent();
            }
            return NotFound();
        }

    }
}
