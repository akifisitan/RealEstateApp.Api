namespace RealEstateApp.Api.DTO.PropertyDTO
{
    public class PropertyCreateResponseDTO : BaseDTO.BaseDTO
    {
        public int Price { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public float Latitude { get; set; }
        public float Longitude { get; set; }
        public int UserId { get; set; }
        public int TypeId { get; set; }
        public int StatusId { get; set; }
        public int CurrencyId { get; set; }
        public string Thumbnail { get; set; }
        public List<string> Images { get; set; }
    }
}
