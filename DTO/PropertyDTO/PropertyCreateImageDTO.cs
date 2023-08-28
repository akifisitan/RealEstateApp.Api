namespace RealEstateApp.Api.DTO.PropertyDTO
{
    public class PropertyCreateImageDTO
    {
        public int PropertyId { get; set; }
        public IFormFile Image { get; set; }
    }
}
