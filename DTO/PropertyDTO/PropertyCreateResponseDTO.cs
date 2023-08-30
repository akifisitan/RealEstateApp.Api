using RealEstateApp.Api.Entity;

namespace RealEstateApp.Api.DTO.PropertyDTO
{
    public class PropertyCreateResponseDTO
    {
        public int Id { get; set; }
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
        public PropertyCreateResponseDTO(Property property, List<string> images)
        {
            Price = property.Price;
            StartDate = property.StartDate;
            EndDate = property.EndDate;
            Latitude = property.Latitude;
            Longitude = property.Longitude;
            UserId = property.UserId;
            TypeId = property.PropertyTypeId;
            StatusId = property.PropertyStatusId;
            CurrencyId = property.CurrencyId;
            Thumbnail = property.Thumbnail;
            Images = images;
        }
    }
}
