namespace RealEstateApp.Api.Entity
{
    public class User : BaseEntity
    {
        public string Name { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public ICollection<Property> Properties { get; set; }

    }
}
