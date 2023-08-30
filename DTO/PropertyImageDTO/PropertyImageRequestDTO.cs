namespace RealEstateApp.Api.DTO.PropertyImageDTO
{
    public class PropertyImageRequestDTO
    {
        public int PropertyId { get; set; }
        public IFormFile Image { get; set; }
    }
}
