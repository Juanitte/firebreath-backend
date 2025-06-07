namespace FireBreath.UsersMicroservice.Models.Dtos.EntityDto
{
    public class LoginDto
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public bool RememberMe { get; set; } = false;

        public LoginDto()
        {
            Email = string.Empty;
            Password = string.Empty;
        }

        public LoginDto(string email, string password)
        {
            Email = email;
            Password = password;
        }

        public LoginDto(string email, string password, bool rememberMe)
        {
            Email = email;
            Password = password;
            RememberMe = rememberMe;
        }
    }
}
