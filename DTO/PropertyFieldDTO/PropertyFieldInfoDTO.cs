using RealEstateApp.Api.Entity;

namespace RealEstateApp.Api.DTO.PropertyFieldDTO
{
    public class PropertyFieldInfoDTO<T> : BaseDTO.BaseDTO where T : PropertyFieldBase
    {
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
