using RealEstateApp.Api.Entity;

namespace RealEstateApp.Api.DTO.UserDTO
{
    public class UserInfoDTO
    {
        public int Id { get; set; }
        public string Username { get; set; }

        public UserInfoDTO()
        {
            Username = string.Empty;
        }
        public UserInfoDTO(User user)
        {
            Id = user.Id;
            Username = user.Username;
        }
    }
}
