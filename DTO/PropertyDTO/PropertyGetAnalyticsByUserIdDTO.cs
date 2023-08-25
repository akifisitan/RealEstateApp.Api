namespace RealEstateApp.Api.DTO.PropertyDTO
{
    public class PropertyGetAnalyticsByUserIdDTO
    {
        public Dictionary<string, int> Currencies { get; set; }
        public Dictionary<string, int> Statuses { get; set; }
        public Dictionary<string, int> Types { get; set; }
    }
}
