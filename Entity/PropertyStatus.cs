namespace RealEstateApp.Api.Entity
{
    public class PropertyStatus : PropertyFieldBase
    {
        public PropertyStatus() { }
        public PropertyStatus(string status)
        {
            Value = status;
        }

        public static List<PropertyStatus> GenerateDefault()
        {
            return new List<PropertyStatus>
            {
                new PropertyStatus("For Sale"),
                new PropertyStatus("For Rent"),
                new PropertyStatus("Sold")
            };
        }
    }
}
