namespace RealEstateApp.Api.DTO.PropertyDTO
{
    public class PropertyGetPaginatedRequestDTO
    {
        public int? Page { get; set; }
        public int? PageSize { get; set; }
        public int? TypeId { get; set; }
        public int? StatusId { get; set; }
        public int? CurrencyId { get; set; }
        public int? MinPrice { get; set; }
        public int? MaxPrice { get; set; }
    }
}
