namespace RealEstateApp.Api.DTO.PropertyFieldDTO
{
    public class PropertyFieldCreateRequestDTO
    {
        public string Value { get; set; }
        public PropertyFieldCreateRequestDTO()
        {
            Value = string.Empty;
        }
        public PropertyFieldCreateRequestDTO(string status)
        {
            Value = status;
        }
    }
}
