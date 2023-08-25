using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RealEstateApp.Api.Auth;
using RealEstateApp.Api.DatabaseContext;
using RealEstateApp.Api.DTO;
using RealEstateApp.Api.DTO.PropertyDTO;
using RealEstateApp.Api.DTO.PropertyFieldDTO;
using RealEstateApp.Api.DTO.UserDTO;
using RealEstateApp.Api.Entity;
using RealEstateApp.Api.Enums;

namespace RealEstateApp.Api.Controllers
{

    [ApiController]
    [Route("[controller]")]
    public class PropertyController : ControllerBase
    {

        private readonly RealEstateContext _context;

        public PropertyController(RealEstateContext context)
        {
            _context = context;
        }

        [Authorize(Roles = UserRoles.User)]
        [HttpGet]
        [Route("getById")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _context.Properties.AsNoTracking()
                .Where(x => x.Id == id && x.Status != (int)EntityStatus.Deleted)
                .Include(x => x.PropertyImages)
                .Include(x => x.User)
                .Include(x => x.Currency)
                .Include(x => x.PropertyStatus)
                .Include(x => x.PropertyType)
                .FirstOrDefaultAsync();
            if (result == null)
            {
                return NotFound();
            }
            var images = new List<PropertyFieldInfoDTO<PropertyImage>>();
            foreach (var image in result.PropertyImages)
            {
                images.Add(new PropertyFieldInfoDTO<PropertyImage>(image));
            }
            var responseDTO = new PropertyGetByIdResponseDTO
            {
                Id = result.Id,
                Price = result.Price,
                StartDate = result.StartDate.ToShortDateString(),
                EndDate = result.EndDate.ToShortDateString(),
                Latitude = result.Latitude,
                Longitude = result.Longitude,
                PropertyImages = images,
                PropertyStatus = new PropertyFieldInfoDTO<PropertyStatus>(result.PropertyStatus),
                PropertyType = new PropertyFieldInfoDTO<PropertyType>(result.PropertyType),
                Currency = new PropertyFieldInfoDTO<Currency>(result.Currency),
                User = new UserInfoDTO(result.User)
            };
            return Ok(responseDTO);
        }

        [Authorize(Roles = UserRoles.User)]
        [HttpGet]
        [Route("getAllByUserId")]
        public async Task<IActionResult> GetAllByUserId()
        {
            int userId = Convert.ToInt32(User.FindFirst("Id")?.Value);
            var result = await _context.Properties.AsNoTracking()
                .Where(x => x.UserId == userId && x.Status != (int)EntityStatus.Deleted)
                .Include(x => x.PropertyImages)
                .Include(x => x.User)
                .Include(x => x.Currency)
                .Include(x => x.PropertyStatus)
                .Include(x => x.PropertyType)
                .ToListAsync();

            if (result == null) return NotFound();

            var responseDTO = new List<PropertyListDTO>();
            foreach (var property in result)
            {
                var image = new PropertyFieldInfoDTO<PropertyImage>
                {
                    Value = property.PropertyImages.First().Value
                };

                var dto = new PropertyListDTO
                {
                    Id = property.Id,
                    Thumbnail = image.Value,
                    Status = property.PropertyStatus.Value,
                    Type = property.PropertyType.Value,
                    Currency = property.Currency.Value,
                    Price = property.Price,
                    Latitude = property.Latitude,
                    Longitude = property.Longitude
                };
                responseDTO.Add(dto);
            }
            return Ok(responseDTO);
        }

        [HttpGet]
        [Route("getAll")]
        public async Task<IActionResult> GetAll()
        {
            var result = await _context.Properties.AsNoTracking()
                .Where(x => x.Status != (int)EntityStatus.Deleted)
                .Include(x => x.PropertyImages)
                .Include(x => x.User)
                .Include(x => x.Currency)
                .Include(x => x.PropertyStatus)
                .Include(x => x.PropertyType)
                .ToListAsync();

            if (result == null) return NotFound();

            var responseDTO = new List<PropertyListDTO>();
            foreach (var property in result)
            {
                var thumbnail = new PropertyFieldInfoDTO<PropertyImage>
                {
                    Value = property.PropertyImages.First().Value
                };
                var dto = new PropertyListDTO
                {
                    Id = property.Id,
                    Thumbnail = thumbnail.Value,
                    Status = property.PropertyStatus.Value,
                    Type = property.PropertyType.Value,
                    Currency = property.Currency.Value,
                    Price = property.Price,
                    Latitude = property.Latitude,
                    Longitude = property.Longitude
                };
                responseDTO.Add(dto);
            }
            return Ok(responseDTO);
        }

        [Authorize(Roles = UserRoles.User)]
        [HttpGet]
        [Route("getPaginated")]
        public async Task<IActionResult> GetPaginated
            (int pageNumber, int? statusId, int? typeId, int? currencyId, int? minPrice, int? maxPrice
            )
        {
            minPrice ??= 0;
            maxPrice ??= int.MaxValue;
            int itemsPerPage = 30;
            var query = _context.Properties.AsNoTracking()
                .Where(x => x.Status != (int)EntityStatus.Deleted)
                .Where(x => x.PropertyTypeId == typeId || typeId == null)
                .Where(x => x.PropertyStatusId == statusId || statusId == null)
                .Where(x => x.CurrencyId == currencyId || currencyId == null)
                .Where(x => x.Price >= minPrice && x.Price <= maxPrice)
                .Include(x => x.PropertyImages)
                .Include(x => x.User)
                .Include(x => x.Currency)
                .Include(x => x.PropertyStatus)
                .Include(x => x.PropertyType);

            var totalItems = await query.CountAsync();
            var numberOfPages = (int)Math.Ceiling((double)totalItems / itemsPerPage);

            var result = await query.Skip(itemsPerPage * (pageNumber - 1)).Take(itemsPerPage).ToListAsync();

            if (result == null)
            {
                return NotFound();
            }
            var responseData = new List<PropertyListDTO>();
            foreach (var property in result)
            {
                var image = new PropertyFieldInfoDTO<PropertyImage>
                {
                    Value = property.PropertyImages.First().Value
                };
                var dto = new PropertyListDTO
                {
                    Id = property.Id,
                    Thumbnail = image.Value,
                    Status = property.PropertyStatus.Value,
                    Type = property.PropertyType.Value,
                    Currency = property.Currency.Value,
                    Price = property.Price,
                    Latitude = property.Latitude,
                    Longitude = property.Longitude
                };
                responseData.Add(dto);
            }
            var responseDTO = new PropertyGetPaginatedResponseDTO()
            {
                Items = responseData,
                NumberOfPages = numberOfPages,
                CurrentPage = pageNumber,
                NumberOfItems = responseData.Count,
                ItemsPerPage = itemsPerPage
            };
            return Ok(responseDTO);
        }

        [HttpGet]
        [Route("getAnalyticsByUserId")]
        public async Task<IActionResult> GetAnalyticsByUserId()
        {
            int userId = Convert.ToInt32(User.FindFirst("Id")?.Value);
            var result = await _context.Properties.AsNoTracking()
                .Where(x => x.UserId == userId && x.Status != (int)EntityStatus.Deleted)
                .Include(x => x.Currency)
                .Include(x => x.PropertyStatus)
                .Include(x => x.PropertyType)
                .ToListAsync();
            if (result == null)
            {
                return NotFound();
            }
            var currencyDict = new Dictionary<string, int>();
            var statusDict = new Dictionary<string, int>();
            var typeDict = new Dictionary<string, int>();
            foreach (var property in result)
            {
                if (currencyDict.ContainsKey(property.Currency.Value))
                {
                    currencyDict[property.Currency.Value] += 1;
                }
                else
                {
                    currencyDict.Add(property.Currency.Value, 1);
                }
                if (statusDict.ContainsKey(property.PropertyStatus.Value))
                {
                    statusDict[property.PropertyStatus.Value] += 1;
                }
                else
                {
                    statusDict.Add(property.PropertyStatus.Value, 1);
                }
                if (typeDict.ContainsKey(property.PropertyType.Value))
                {
                    typeDict[property.PropertyType.Value] += 1;
                }
                else
                {
                    typeDict.Add(property.PropertyType.Value, 1);
                }
            }
            var responseDTO = new PropertyGetAnalyticsByUserIdDTO
            {
                Currencies = currencyDict,
                Statuses = statusDict,
                Types = typeDict
            };

            return Ok(responseDTO);
        }

        [Authorize(Roles = UserRoles.User)]
        [HttpPost]
        public async Task<IActionResult> Create([FromForm] PropertyCreateRequestDTO request)
        {
            var response = new GenericResponse<PropertyCreateResponseDTO>();
            if (!DateTime.TryParseExact(request.StartDate, "dd/MM/yyyy",
                System.Globalization.CultureInfo.InvariantCulture,
                System.Globalization.DateTimeStyles.None, out DateTime parsedStartDate) ||
                !DateTime.TryParseExact(request.EndDate, "dd/MM/yyyy",
                System.Globalization.CultureInfo.InvariantCulture,
                System.Globalization.DateTimeStyles.None, out DateTime parsedEndDate))
            {
                response.Message = "Please enter a valid date in dd/MM/yyyy format.";
                return BadRequest(response);
            }
            if (parsedStartDate > parsedEndDate)
            {
                response.Message = "Please make sure the start date is earlier than the end date.";
                return BadRequest(response);
            }
            int userId = Convert.ToInt32(User.FindFirst("Id")?.Value);
            var newProperty = new Property
            {
                StartDate = parsedStartDate,
                EndDate = parsedEndDate,
                Latitude = request.Latitude,
                Longitude = request.Longitude,
                PropertyTypeId = request.PropertyTypeId,
                PropertyStatusId = request.PropertyStatusId,
                CurrencyId = request.CurrencyId,
                Price = request.Price,
                UserId = userId
            };
            var imageStrings = new List<string>();
            foreach (var file in request.Photos)
            {
                if (file.Length > 0)
                {
                    using var memoryStream = new MemoryStream();
                    await file.CopyToAsync(memoryStream);

                    // upload the file if less than 4 mb  
                    if (memoryStream.Length < 4 * 1024 * 1024)
                    {
                        var photo = Convert.ToBase64String(memoryStream.ToArray());
                        imageStrings.Add(photo);
                    }
                    else
                    {
                        response.Message = "One or more of the files is too large.";
                        return BadRequest(response);
                    }
                }
            }
            if (imageStrings.Count == 0)
            {
                response.Message = "Please upload at least one image.";
                return BadRequest(response);
            }
            var addedProperty = _context.Properties.Add(newProperty);
            await _context.SaveChangesAsync();
            var propertyId = addedProperty.Entity.Id;
            foreach (var imageString in imageStrings)
            {
                var newImage = new PropertyImage
                {
                    PropertyId = propertyId,
                    Value = imageString
                };
                _context.PropertyImages.Add(newImage);
            }
            await _context.SaveChangesAsync();
            var data = new PropertyCreateResponseDTO
            {
                Id = propertyId,
                StartDate = parsedStartDate,
                EndDate = parsedEndDate,
                Latitude = request.Latitude,
                Longitude = request.Longitude,
                TypeId = request.PropertyTypeId,
                StatusId = request.PropertyStatusId,
                CurrencyId = request.CurrencyId,
                Price = request.Price,
                UserId = userId,
                Images = imageStrings
            };
            response.Data = data;
            response.Message = "Property Creation Successful.";
            return Ok(response);
        }

        [Authorize(Roles = UserRoles.User)]
        [HttpPut]
        public async Task<IActionResult> Update([FromForm] PropertyUpdateRequestDTO request)
        {
            var response = new GenericResponse<PropertyUpdateResponseDTO>();
            var item = await _context.Properties
                .Where(x => x.Id == request.Id && x.Status != (int)EntityStatus.Deleted)
                .Include(x => x.PropertyImages)
                .Include(x => x.User)
                .Include(x => x.Currency)
                .Include(x => x.PropertyStatus)
                .Include(x => x.PropertyType)
                .SingleOrDefaultAsync();

            if (item == null)
            {
                response.Message = "No property found with the given id.";
                return NotFound(response);
            }
            int userId = Convert.ToInt32(User.FindFirst("Id")?.Value);
            if (userId != item.UserId && !User.IsInRole(UserRoles.Admin))
            {
                response.Message = "You are not authorized to update this property.";
                return Unauthorized(response);
            }
            if (!DateTime.TryParseExact(request.StartDate, "dd/MM/yyyy",
                System.Globalization.CultureInfo.InvariantCulture,
                System.Globalization.DateTimeStyles.None, out DateTime parsedStartDate) ||
                !DateTime.TryParseExact(request.EndDate, "dd/MM/yyyy",
                System.Globalization.CultureInfo.InvariantCulture,
                System.Globalization.DateTimeStyles.None, out DateTime parsedEndDate))
            {
                response.Message = "Please enter a valid date in MM/dd/yyyy format.";
                return BadRequest(response);
            }
            if (parsedStartDate > parsedEndDate)
            {
                response.Message = "Please make sure the start date is earlier than the end date.";
                return BadRequest(response);
            }
            var imageStrings = new List<string>();

            if (request.Photos != null)
            {
                foreach (var file in request.Photos)
                {
                    if (file.Length > 0)
                    {
                        using var memoryStream = new MemoryStream();
                        await file.CopyToAsync(memoryStream);

                        // upload the file if less than 4 mb  
                        if (memoryStream.Length < 4 * 1024 * 1024)
                        {
                            var photo = Convert.ToBase64String(memoryStream.ToArray());
                            imageStrings.Add(photo);
                        }
                        else
                        {
                            response.Message = "One or more of the images is too large.";
                            return BadRequest(response);
                        }
                    }
                }
                if (imageStrings.Count == 0)
                {
                    response.Message = "Please upload at least one image.";
                    return BadRequest(response);
                }
                foreach (var imageString in imageStrings)
                {
                    var newImage = new PropertyImage
                    {
                        PropertyId = request.Id,
                        Value = imageString
                    };
                    _context.PropertyImages.Add(newImage);
                }
            }
            var propertyImages = new List<string>();
            foreach (var image in item.PropertyImages)
            {
                propertyImages.Add(image.Value);
            }
            if (imageStrings.Count > 0)
            {
                foreach (var image in imageStrings)
                {
                    propertyImages.Add(image);
                }
            }
            item.Price = request.Price != null ? (int)request.Price : item.Price;
            item.StartDate = parsedStartDate;
            item.EndDate = parsedEndDate;
            item.Latitude = request.Latitude != null ? (float)request.Latitude : item.Latitude;
            item.Longitude = request.Longitude != null ? (float)request.Longitude : item.Longitude;
            item.PropertyTypeId = request.PropertyTypeId != null ? (int)request.PropertyTypeId : item.PropertyTypeId;
            item.PropertyTypeId = request.PropertyTypeId != null ? (int)request.PropertyTypeId : item.PropertyTypeId;
            item.PropertyStatusId = request.PropertyStatusId != null ? (int)request.PropertyStatusId : item.PropertyStatusId;
            item.CurrencyId = request.CurrencyId != null ? (int)request.CurrencyId : item.CurrencyId;
            await _context.SaveChangesAsync();

            var data = new PropertyUpdateResponseDTO
            {
                Id = item.Id,
                StartDate = parsedStartDate,
                EndDate = parsedEndDate,
                Latitude = item.Latitude,
                Longitude = item.Longitude,
                TypeId = item.PropertyTypeId,
                StatusId = item.PropertyStatusId,
                CurrencyId = item.CurrencyId,
                Price = item.Price,
                Images = propertyImages
            };
            response.Data = data;
            response.Message = "Property Update Successful.";
            return Ok(response);

        }

        [Authorize(Roles = UserRoles.User)]
        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            int userId = Convert.ToInt32(User.FindFirst("Id")?.Value);
            var item = await _context.Properties
                .SingleOrDefaultAsync(x => x.Id == id && x.Status != (int)EntityStatus.Deleted);
            if (item == null)
            {
                return NotFound();
            }
            if (item.UserId != userId && !User.IsInRole(UserRoles.Admin))
            {
                return Unauthorized();
            }
            item.Status = (int)EntityStatus.Deleted;
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
