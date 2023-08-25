namespace RealEstateApp.Api.DTO.PropertyDTO
{
    public class PropertyFilterRequestDTO
    {
        public int StatusId { get; set; }
        public int TypeId { get; set; }
        public int CurrencyId { get; set; }
        public int MinPrice { get; set; }
        public int MaxPrice { get; set; }
    }
}
