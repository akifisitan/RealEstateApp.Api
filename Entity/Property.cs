namespace RealEstateApp.Api.Entity
{
    public class Property : BaseEntity
    {
        public PropertyType PropertyType { get; set; }
        public PropertyStatus PropertyStatus { get; set; }
        public Currency Currency { get; set; }
        public int Price { get; set; }
        public string Thumbnail { get; set; }
        public float Latitude { get; set; }
        public float Longitude { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public User User { get; set; }
        public int UserId { get; set; }
        public int PropertyTypeId { get; set; }
        public int PropertyStatusId { get; set; }
        public int CurrencyId { get; set; }
        public ICollection<PropertyImage> PropertyImages { get; set; }
    }
}
