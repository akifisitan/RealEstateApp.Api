namespace RealEstateApp.Api.DTO.PropertyDTO
{
    public class PropertyListDTO
    {
        public int Id { get; set; }
        public string Currency { get; set; }
        public string Status { get; set; }
        public int Price { get; set; }
        public float Latitude { get; set; }
        public float Longitude { get; set; }
        public string Type { get; set; }
        public string Thumbnail { get; set; }
    }
}
