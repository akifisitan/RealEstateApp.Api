namespace RealEstateApp.Api.DTO.PropertyImageDTO
{
    public class PropertyCreateImageRequestDTO
    {
        public int PropertyId { get; set; }
        public List<IFormFile> Images { get; set; }
    }
}
