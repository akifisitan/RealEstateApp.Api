namespace RealEstateApp.Api.DTO.PropertyFieldDTO
{
    public class PropertyFieldUpdateRequestDTO : BaseDTO.BaseDTO
    {
        public string Value { get; set; }

        public PropertyFieldUpdateRequestDTO()
        {
            Value = string.Empty;
        }
    }
}
