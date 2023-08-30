namespace RealEstateApp.Api.DTO.PropertyFieldDTO
{
    public class PropertyFieldUpdateRequestDTO
    {
        public int Id { get; set; }
        public string Value { get; set; }

        public PropertyFieldUpdateRequestDTO()
        {
            Value = string.Empty;
        }
    }
}
