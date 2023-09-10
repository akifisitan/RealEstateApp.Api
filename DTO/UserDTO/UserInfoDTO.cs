using RealEstateApp.Api.Entity;

namespace RealEstateApp.Api.DTO.UserDTO
{
    public class UserInfoDTO
    {
        public string Username { get; set; }
        public string Email { get; set; }


        public UserInfoDTO()
        {
            Username = string.Empty;
            Email = string.Empty;
        }
        public UserInfoDTO(User user)
        {
            Username = user.Username;
            Email = user.Email;
        }
    }
}
