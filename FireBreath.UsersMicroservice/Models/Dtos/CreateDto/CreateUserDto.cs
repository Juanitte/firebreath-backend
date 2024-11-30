using Common.Utilities;
using EasyWeb.UserMicroservice.Models.Entities;

namespace EasyWeb.UserMicroservice.Models.Dtos.CreateDto
{
    public class CreateUserDto
    {
        public string Tag {  get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string PhoneNumber { get; set; }
        public string FullName { get; set; }
        public Country Country { get; set; }
        public Language Language { get; set; }
        public DateTime Created { get; set; } = DateTime.Now;
        public bool IsBanned { get; set; } = false;
        public string Role { get; set; } = "User";

        public CreateUserDto()
        {
            this.Tag = string.Empty;
            this.UserName = string.Empty;
            this.Email = string.Empty;
            this.PhoneNumber = string.Empty;
            this.FullName = string.Empty;
            this.Country = Country.UNDEFINED;
            this.Language = Language.English;
            this.Password = string.Empty;
        }

        public CreateUserDto(string tag, string userName, string password, string email, string phoneNumber, string fullName, Country country, Language language)
        {
            this.Tag = tag;
            this.UserName = userName;
            this.Email = email;
            this.Password = password;
            this.PhoneNumber = phoneNumber;
            this.FullName = fullName;
            this.Country = country;
            this.Language = language;
        }
    }
}
