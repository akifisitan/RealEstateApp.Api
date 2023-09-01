namespace RealEstateApp.Api.DTO.PropertyImageDTO
{
    public class PropertyUpdateImageRequestDTO
    {
        public int PropertyId { get; set; }
        public IFormFile Image { get; set; }
    }
}
