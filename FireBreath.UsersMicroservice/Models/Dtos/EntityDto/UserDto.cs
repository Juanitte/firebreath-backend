using Common.Utilities;

namespace EasyWeb.UserMicroservice.Models.Dtos.EntityDto
{
    public class UserDto
    {
        public int Id { get; set; }
        public string Tag { get; set; }
        public string FullName { get; set; }
        public Country Country { get; set; }
        public Language Language { get; set; }
        public DateTime Created { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Role { get; set; }
        public bool IsBanned { get; set; }

        public UserDto()
        {
            Id = 0;
            Tag = string.Empty;
            FullName = string.Empty;
            Country = Country.UNDEFINED;
            Language = 0;
            Created = DateTime.Now;
            UserName = string.Empty;
            Email = string.Empty;
            PhoneNumber = string.Empty;
            Role = string.Empty;
            IsBanned = false;
        }
    }
}
