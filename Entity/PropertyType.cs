namespace RealEstateApp.Api.Entity
{
    public class PropertyType : PropertyFieldBase
    {
        public PropertyType() { }
        public PropertyType(string type)
        {
            Value = type;
        }

        public static List<PropertyType> GenerateDefault()
        {
            return new List<PropertyType>
            {
                new PropertyType("Apartment"),
                new PropertyType("House"),
                new PropertyType("Townhome"),
                new PropertyType("Land"),
                new PropertyType("Villa")
            };
        }
    }
}
