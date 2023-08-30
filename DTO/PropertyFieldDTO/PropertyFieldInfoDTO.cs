using RealEstateApp.Api.Entity;

namespace RealEstateApp.Api.DTO.PropertyFieldDTO
{
    public class PropertyFieldInfoDTO<T> where T : PropertyFieldBase
    {
        public int Id { get; set; }
        public string Value { get; set; }

        public PropertyFieldInfoDTO()
        {
            Value = string.Empty;
        }

        public PropertyFieldInfoDTO(T propertyField)
        {
            Id = propertyField.Id;
            Value = propertyField.Value;
        }
    }
}
