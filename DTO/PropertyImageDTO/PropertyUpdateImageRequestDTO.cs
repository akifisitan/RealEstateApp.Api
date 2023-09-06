namespace RealEstateApp.Api.DTO.PropertyImageDTO
{
    public class PropertyUpdateImageRequestDTO
    {
        public int Id { get; set; }
        public IFormFile Image { get; set; }
    }
}
