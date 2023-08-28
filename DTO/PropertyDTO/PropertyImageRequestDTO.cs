namespace RealEstateApp.Api.DTO.PropertyDTO
{
    public class PropertyImageRequestDTO
    {
        public int PropertyId { get; set; }
        public IFormFile Image { get; set; }
    }
}
