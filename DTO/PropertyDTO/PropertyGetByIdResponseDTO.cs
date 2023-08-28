namespace RealEstateApp.Api.DTO.PropertyDTO
{
    public class PropertyGetByIdResponseDTO : BaseDTO.BaseDTO
    {
        public int Price { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public float Latitude { get; set; }
        public float Longitude { get; set; }
        public string Currency { get; set; }
        public string Status { get; set; }
        public string Type { get; set; }
        public List<string> Images { get; set; }

    }
}
