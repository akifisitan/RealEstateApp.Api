using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RealEstateApp.Api.Auth;
using RealEstateApp.Api.DatabaseContext;
using RealEstateApp.Api.DTO.PropertyFieldDTO;
using RealEstateApp.Api.DTO.PropertyImageDTO;
using RealEstateApp.Api.Entity;
using RealEstateApp.Api.Enums;
using SixLabors.ImageSharp.Formats.Jpeg;

namespace RealEstateApp.Api.Controllers
{

    [ApiController]
    [Route("[controller]")]
    public class PropertyImageController : ControllerBase
    {

        private readonly RealEstateContext _context;
        private readonly DbSet<PropertyImage> _set;

        public PropertyImageController(RealEstateContext context)
        {
            _context = context;
            _set = _context.PropertyImages;
        }

        [HttpGet]
        [Route("list")]
        public async Task<IActionResult> List()
        {
            var result = await _set
                .AsNoTracking()
                .Where(p => p.Status != (int)EntityStatus.Deleted)
                .ToListAsync();

            if (result == null)
                return NotFound();

            var imageInfoList = new List<PropertyFieldInfoDTO<PropertyImage>>();
            result.ForEach(image => imageInfoList.Add(new PropertyFieldInfoDTO<PropertyImage>(image)));

            return Ok(imageInfoList);
        }

        [Authorize(Roles = UserRoles.User)]
        [HttpPost]
        public async Task<IActionResult> Create(PropertyImageRequestDTO request)
        {
            var property = _context.Properties
                .SingleOrDefault(x => x.Id == request.PropertyId && x.Status != (int)EntityStatus.Deleted);
            if (property == null) return NotFound();

            int userId = Convert.ToInt32(User.FindFirst("Id")?.Value);
            if (property.UserId != userId && !User.IsInRole(UserRoles.Admin)) return Unauthorized();

            var file = request.Image;
            if (file.Length > 0)
            {
                using var memoryStream = new MemoryStream();
                await file.CopyToAsync(memoryStream);
                if (memoryStream.Length < 2 * 1024 * 1024)
                {
                    using var newImage = Image.Load(memoryStream.ToArray());
                    newImage.Mutate(x => x.Resize(500, 0));
                    var newPropertyImage = new PropertyImage
                    {
                        PropertyId = request.PropertyId,
                        Value = newImage.ToBase64String(JpegFormat.Instance)
                    };
                    _context.PropertyImages.Add(newPropertyImage);
                    await _context.SaveChangesAsync();
                    return Ok(new PropertyFieldInfoDTO<PropertyImage>(newPropertyImage));
                }
            }
            return BadRequest();

        }

        [Authorize(Roles = UserRoles.User)]
        [HttpPut]
        public async Task<IActionResult> Update(PropertyImageRequestDTO request)
        {
            var image = await _context.PropertyImages
                .SingleOrDefaultAsync(x => x.Id == request.PropertyId && x.Status != (int)EntityStatus.Deleted);
            if (image == null) return NotFound();

            var property = await _context.Properties
                .SingleOrDefaultAsync(x => x.Id == image.PropertyId && x.Status != (int)EntityStatus.Deleted);
            if (property == null) return NotFound();

            int userId = Convert.ToInt32(User.FindFirst("Id")?.Value);
            if (property.UserId != userId && !User.IsInRole(UserRoles.Admin)) return Unauthorized();

            var file = request.Image;
            if (file.Length > 0)
            {
                using var memoryStream = new MemoryStream();
                await file.CopyToAsync(memoryStream);
                if (memoryStream.Length < 2 * 1024 * 1024)
                {
                    using var newImage = Image.Load(memoryStream.ToArray());
                    newImage.Mutate(x => x.Resize(500, 0));
                    image.Value = newImage.ToBase64String(JpegFormat.Instance);
                    await _context.SaveChangesAsync();
                    return NoContent();
                }
            }
            return BadRequest();
        }

        [Authorize(Roles = UserRoles.User)]
        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            var image = await _context.PropertyImages
                .SingleOrDefaultAsync(x => x.Id == id && x.Status != (int)EntityStatus.Deleted);
            if (image == null) return NotFound();

            int userId = Convert.ToInt32(User.FindFirst("Id")?.Value);
            var item = await _context.Properties
                .SingleOrDefaultAsync(x => x.Id == image.PropertyId && x.Status != (int)EntityStatus.Deleted);
            if (item == null) return NotFound();

            if (item.UserId != userId && !User.IsInRole(UserRoles.Admin)) return Unauthorized();

            image.Status = (int)EntityStatus.Deleted;
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
