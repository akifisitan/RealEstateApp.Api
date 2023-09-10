using RealEstateApp.Api.DTO.PropertyFieldDTO;
using RealEstateApp.Api.DTO.UserDTO;
using RealEstateApp.Api.Entity;

namespace RealEstateApp.Api.DTO.PropertyDTO
{
    public class PropertyDetailedResponseDTO
    {
        public int Id { get; set; }
        public int Price { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public float Latitude { get; set; }
        public float Longitude { get; set; }
        public string Currency { get; set; }
        public string Status { get; set; }
        public string Type { get; set; }
        public UserInfoDTO Owner { get; set; }
        public List<PropertyFieldInfoDTO<PropertyImage>> Images { get; set; }

    }
}
