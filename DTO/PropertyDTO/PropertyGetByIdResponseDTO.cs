using RealEstateApp.Api.DTO.PropertyFieldDTO;
using RealEstateApp.Api.DTO.UserDTO;
using RealEstateApp.Api.Entity;

namespace RealEstateApp.Api.DTO.PropertyDTO
{
    public class PropertyGetByIdResponseDTO : BaseDTO.BaseDTO
    {
        public int Price { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public float Latitude { get; set; }
        public float Longitude { get; set; }
        public PropertyFieldInfoDTO<Currency> Currency { get; set; }
        public PropertyFieldInfoDTO<PropertyStatus> PropertyStatus { get; set; }
        public PropertyFieldInfoDTO<PropertyType> PropertyType { get; set; }
        public List<PropertyFieldInfoDTO<PropertyImage>> PropertyImages { get; set; }
        public UserInfoDTO User { get; set; }

    }
}
