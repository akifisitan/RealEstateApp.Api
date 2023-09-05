using System.Text.RegularExpressions;

namespace RealEstateApp.Api.DTO.AuthDTO
{
    public class RegisterRequestDTO
    {
        public string Username { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }

        private bool CheckUsername()
        {
            return Regex.IsMatch(Username, pattern: "^[a-z0-9]+$");
        }

        private bool CheckEmail()
        {
            return Regex.IsMatch(Email, pattern: "[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?");
        }

        private bool CheckPassword()
        {
            // Check for at least 1 capital letter
            if (!Regex.IsMatch(Password, pattern: "[A-Z]"))
                return false;

            // Check for at least 1 lowercase letter
            if (!Regex.IsMatch(Password, pattern: "[a-z]"))
                return false;

            // Check for at least 1 of the specified symbols
            if (!Regex.IsMatch(Password, pattern: "[.,-;]"))
                return false;

            // Check for at least 1 number
            if (!Regex.IsMatch(Password, pattern: "[0-9]"))
                return false;

            // Check the length condition
            if (Password.Length < 8 || Password.Length > 12)
                return false;

            // All conditions are satisfied
            return true;
        }

        public bool IsValid()
        {
            Username = Username.Trim();
            Email = Email.Trim();
            Password = Password.Trim();
            return CheckUsername() && CheckEmail() && CheckPassword();
        }


    }
}
