namespace RealEstateApp.Api.DTO.PropertyDTO
{
    public class PropertyGetPaginatedResponseDTO
    {
        public int CurrentPage { get; set; }
        public int NumberOfPages { get; set; }
        public List<PropertyShowcaseResponseDTO> Items { get; set; }
    }
}
