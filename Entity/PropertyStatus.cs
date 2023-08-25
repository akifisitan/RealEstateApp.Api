namespace RealEstateApp.Api.Entity
{
    public class PropertyStatus : PropertyFieldBase
    {
        public PropertyStatus() { }
        public PropertyStatus(string status)
        {
            Value = status;
        }
    }
}
