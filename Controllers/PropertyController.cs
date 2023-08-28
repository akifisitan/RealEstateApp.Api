﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RealEstateApp.Api.Auth;
using RealEstateApp.Api.DatabaseContext;
using RealEstateApp.Api.DTO;
using RealEstateApp.Api.DTO.PropertyDTO;
using RealEstateApp.Api.DTO.PropertyFieldDTO;
using RealEstateApp.Api.Entity;
using RealEstateApp.Api.Enums;
using SixLabors.ImageSharp.Formats.Jpeg;

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

        [HttpGet]
        [Route("getById")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _context.Properties.AsNoTracking()
                .Where(x => x.Id == id && x.Status != (int)EntityStatus.Deleted)
                .Include(x => x.PropertyImages)
                .Include(x => x.Currency)
                .Include(x => x.PropertyStatus)
                .Include(x => x.PropertyType)
                .FirstOrDefaultAsync();

            if (result == null) return NotFound();

            var images = new List<string>();
            foreach (var image in result.PropertyImages)
            {
                if (image.Status != (int)EntityStatus.Deleted)
                    images.Add(image.Value);
            }
            var responseDTO = new PropertyGetByIdResponseDTO
            {
                Id = result.Id,
                Price = result.Price,
                StartDate = result.StartDate.ToShortDateString(),
                EndDate = result.EndDate.ToShortDateString(),
                Latitude = result.Latitude,
                Longitude = result.Longitude,
                Images = images,
                Status = result.PropertyStatus.Value,
                Type = result.PropertyType.Value,
                Currency = result.Currency.Value,
            };
            return Ok(responseDTO);
        }

        [Authorize(Roles = UserRoles.User)]
        [HttpGet]
        [Route("getUserShowcaseData")]
        public async Task<IActionResult> GetUserShowcaseData()
        {
            int userId = Convert.ToInt32(User.FindFirst("Id")?.Value);
            var result = await _context.Properties.AsNoTracking()
                .Where(x => x.UserId == userId && x.Status != (int)EntityStatus.Deleted)
                .Include(x => x.Currency)
                .Include(x => x.PropertyStatus)
                .Include(x => x.PropertyType)
                .ToListAsync();

            if (result == null) return NotFound();

            var responseDTO = new List<PropertyShowcaseResponseDTO>();
            foreach (var property in result)
            {
                var dto = new PropertyShowcaseResponseDTO
                {
                    Id = property.Id,
                    Thumbnail = property.Thumbnail,
                    Status = property.PropertyStatus.Value,
                    Type = property.PropertyType.Value,
                    Currency = property.Currency.Value,
                    Price = property.Price,
                };
                responseDTO.Add(dto);
            }
            return Ok(responseDTO);
        }

        [HttpGet]
        [Route("getAllShowcaseData")]
        public async Task<IActionResult> GetAllShowcaseData()
        {
            var result = await _context.Properties.AsNoTracking()
                .Where(x => x.Status != (int)EntityStatus.Deleted)
                .Include(x => x.Currency)
                .Include(x => x.PropertyStatus)
                .Include(x => x.PropertyType)
                .ToListAsync();

            if (result == null) return NotFound();

            var responseDTO = new List<PropertyShowcaseResponseDTO>();
            foreach (var property in result)
            {
                var dto = new PropertyShowcaseResponseDTO
                {
                    Id = property.Id,
                    Thumbnail = property.Thumbnail,
                    Status = property.PropertyStatus.Value,
                    Type = property.PropertyType.Value,
                    Currency = property.Currency.Value,
                    Price = property.Price,
                };
                responseDTO.Add(dto);
            }
            return Ok(responseDTO);
        }

        [Authorize(Roles = UserRoles.User)]
        [HttpGet]
        [Route("getAllUserMapData")]
        public async Task<IActionResult> GetAllUserMapData()
        {
            int userId = Convert.ToInt32(User.FindFirst("Id")?.Value);
            var result = await _context.Properties.AsNoTracking()
                .Where(x => x.UserId == userId && x.Status != (int)EntityStatus.Deleted)
                .Include(x => x.Currency)
                .Include(x => x.PropertyStatus)
                .Include(x => x.PropertyType)
                .ToListAsync();

            if (result == null) return NotFound();

            var responseDTO = new List<PropertyGetMapDataResponseDTO>();
            foreach (var property in result)
            {
                var dto = new PropertyGetMapDataResponseDTO
                {
                    Id = property.Id,
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
        [Route("getAllMapData")]
        public async Task<IActionResult> GetAllMapData()
        {
            var result = await _context.Properties.AsNoTracking()
                .Where(x => x.Status != (int)EntityStatus.Deleted)
                .Include(x => x.Currency)
                .Include(x => x.PropertyStatus)
                .Include(x => x.PropertyType)
                .ToListAsync();

            if (result == null) return NotFound();

            var responseDTO = new List<PropertyGetMapDataResponseDTO>();
            foreach (var property in result)
            {
                var dto = new PropertyGetMapDataResponseDTO
                {
                    Id = property.Id,
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
            var responseData = new List<PropertyShowcaseResponseDTO>();
            foreach (var property in result)
            {
                var dto = new PropertyShowcaseResponseDTO
                {
                    Id = property.Id,
                    Thumbnail = property.Thumbnail,
                    Status = property.PropertyStatus.Value,
                    Type = property.PropertyType.Value,
                    Currency = property.Currency.Value,
                    Price = property.Price,
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

        [HttpPost]
        [Route("migrateOldImages")]
        public async Task<IActionResult> Test()
        {
            var result = await _context.Properties
                .Where(x => x.Status != (int)EntityStatus.Deleted)
                .Include(x => x.PropertyImages)
                .Include(x => x.Currency)
                .Include(x => x.PropertyStatus)
                .Include(x => x.PropertyType)
                .ToListAsync();

            if (result == null) return NotFound();

            foreach (var property in result)
            {
                foreach (var image in property.PropertyImages)
                {
                    if (!image.Value.StartsWith("data:image"))
                    {
                        var imageBytes = Convert.FromBase64String(image.Value);
                        using MemoryStream memoryStream = new(imageBytes);
                        using var imageInstance = Image.Load(memoryStream.ToArray());
                        imageInstance.Mutate(x => x.Resize(500, 0));
                        image.Value = imageInstance.ToBase64String(JpegFormat.Instance);
                    }
                }
                var dataBase64String = property.PropertyImages.First().Value;
                var base64String = dataBase64String.Split(',')[1];
                var thumbnailBytes = Convert.FromBase64String(base64String);
                using MemoryStream thumbnailStream = new(thumbnailBytes);

                using var thumbnail = Image.Load(thumbnailStream.ToArray());
                thumbnail.Mutate(x => x.Resize(320, 240, KnownResamplers.Lanczos3));
                var thumbnailString = thumbnail.ToBase64String(JpegFormat.Instance);
                property.Thumbnail = thumbnailString;
                await _context.SaveChangesAsync();
            }
            return Ok();

        }

        [Authorize(Roles = UserRoles.User)]
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
            if (!DateTime.TryParseExact(request.EndDate, "dd/MM/yyyy",
                System.Globalization.CultureInfo.InvariantCulture,
                System.Globalization.DateTimeStyles.None, out DateTime parsedEndDate))
            {
                response.Message = "Please enter a valid date in dd/MM/yyyy format.";
                return BadRequest(response);
            }
            var startDate = DateTime.Now;
            if (startDate >= parsedEndDate)
            {
                response.Message = "Please make sure the end date is later than today.";
                return BadRequest(response);
            }
            var propertyType = _context.PropertyTypes.SingleOrDefault(x => x.Id == request.PropertyTypeId && x.Status != (int)EntityStatus.Deleted);
            if (propertyType == null)
            {
                response.Message = "Please enter a valid property type id.";
                return BadRequest(response);
            }
            var propertyStatus = _context.PropertyStatuses.SingleOrDefault(x => x.Id == request.PropertyStatusId && x.Status != (int)EntityStatus.Deleted);
            if (propertyStatus == null)
            {
                response.Message = "Please enter a valid property status id.";
                return BadRequest(response);
            }
            var currency = _context.Currencies.SingleOrDefault(x => x.Id == request.CurrencyId && x.Status != (int)EntityStatus.Deleted);
            if (currency == null)
            {
                response.Message = "Please enter a valid currency id.";
                return BadRequest(response);
            }
            if (request.Photos.Count < 1)
            {
                response.Message = "Please upload at least one image.";
                return BadRequest(response);
            }
            var imageStrings = new List<string>();
            foreach (var file in request.Photos)
            {
                if (file.Length > 0)
                {
                    using var memoryStream = new MemoryStream();
                    await file.CopyToAsync(memoryStream);

                    if (memoryStream.Length < 2 * 1024 * 1024)
                    {
                        using var image = Image.Load(memoryStream.ToArray());
                        image.Mutate(x => x.Resize(500, 0));
                        var b64 = image.ToBase64String(JpegFormat.Instance);
                        imageStrings.Add(b64);
                    }
                    else
                    {
                        response.Message = "One or more of the uploaded images is too large.";
                        return BadRequest(response);
                    }
                }
            }
            if (imageStrings.Count == 0)
            {
                response.Message = "Please upload at least one valid image.";
                return BadRequest(response);
            }
            using var thumbnailStream = new MemoryStream();
            await request.Photos[0].CopyToAsync(thumbnailStream);
            using var thumbnail = Image.Load(thumbnailStream.ToArray());
            thumbnail.Mutate(x => x.Resize(320, 240, KnownResamplers.Lanczos3));

            int userId = Convert.ToInt32(User.FindFirst("Id")?.Value);

            var newProperty = new Property
            {
                StartDate = startDate,
                EndDate = parsedEndDate,
                Thumbnail = thumbnail.ToBase64String(JpegFormat.Instance),
                Latitude = request.Latitude,
                Longitude = request.Longitude,
                PropertyTypeId = request.PropertyTypeId,
                PropertyStatusId = request.PropertyStatusId,
                CurrencyId = request.CurrencyId,
                Price = request.Price,
                UserId = userId
            };
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
            response.Data = new PropertyCreateResponseDTO(newProperty, imageStrings);
            response.Message = "Property created successfully.";
            return Ok(response);
        }

        [Authorize(Roles = UserRoles.User)]
        [HttpPut]
        public async Task<IActionResult> Update([FromBody] PropertyUpdateRequestDTO request)
        {
            var property = await _context.Properties
                .Where(x => x.Id == request.Id && x.Status != (int)EntityStatus.Deleted)
                .Include(x => x.User)
                .Include(x => x.Currency)
                .Include(x => x.PropertyStatus)
                .Include(x => x.PropertyType)
                .SingleOrDefaultAsync();

            if (property == null)
            {
                return NotFound("No property found with the given id.");
            }
            int userId = Convert.ToInt32(User.FindFirst("Id")?.Value);
            if (userId != property.UserId && !User.IsInRole(UserRoles.Admin))
            {
                return Unauthorized("You are not authorized to update this property.");
            }
            var valid = DateTime.TryParseExact(request.EndDate, "dd/MM/yyyy",
                System.Globalization.CultureInfo.InvariantCulture,
                System.Globalization.DateTimeStyles.None, out DateTime parsedEndDate);
            if (request.EndDate != null && !valid)
            {
                return BadRequest("Please enter a valid date in dd/MM/yyyy format.");
            }
            if (valid && property.StartDate > parsedEndDate)
            {
                return BadRequest("Please make sure the end date is later than the start date.");
            }
            else
            {
                property.EndDate = parsedEndDate;
            }
            property.Price = request.Price ?? property.Price;
            property.Latitude = request.Latitude ?? property.Latitude;
            property.Longitude = request.Longitude ?? property.Longitude;
            if (request.PropertyTypeId != null)
            {
                var propertyType = _context.PropertyTypes.SingleOrDefault(x => x.Id == request.PropertyTypeId && x.Status != (int)EntityStatus.Deleted);
                if (propertyType == null)
                {
                    return BadRequest("Please enter a valid property type id.");
                }
                property.PropertyTypeId = propertyType.Id;
            }
            if (request.PropertyStatusId != null)
            {
                var propertyStatus = _context.PropertyStatuses.SingleOrDefault(x => x.Id == request.PropertyStatusId && x.Status != (int)EntityStatus.Deleted);
                if (propertyStatus == null)
                {
                    return BadRequest("Please enter a valid property status id.");
                }
                property.PropertyStatusId = propertyStatus.Id;
            }
            if (request.CurrencyId != null)
            {
                var currency = _context.Currencies.SingleOrDefault(x => x.Id == request.CurrencyId && x.Status != (int)EntityStatus.Deleted);
                if (currency == null)
                {
                    return BadRequest("Please enter a valid currency id.");
                }
                property.CurrencyId = currency.Id;
            }
            await _context.SaveChangesAsync();
            return NoContent();

        }

        [Authorize(Roles = UserRoles.User)]
        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            int userId = Convert.ToInt32(User.FindFirst("Id")?.Value);
            var item = await _context.Properties
                .SingleOrDefaultAsync(x => x.Id == id && x.Status != (int)EntityStatus.Deleted);
            if (item == null) return NotFound();

            if (item.UserId != userId && !User.IsInRole(UserRoles.Admin)) return Unauthorized();

            item.Status = (int)EntityStatus.Deleted;
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
