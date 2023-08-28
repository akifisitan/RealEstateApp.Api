namespace RealEstateApp.Api.DTO.PropertyDTO
{
    public class PropertyGetPaginatedResponseDTO
    {
        public int NumberOfPages { get; set; }
        public int CurrentPage { get; set; }
        public int ItemsPerPage { get; set; }
        public int NumberOfItems { get; set; }
        public List<PropertyShowcaseResponseDTO> Items { get; set; }
    }
}
