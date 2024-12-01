using Common.Dtos;
using System.ComponentModel;

namespace FireBreath.UserMicroservice.Models.Dtos.FilterDto
{
    public class UserFilterRequestDto : GenericFilterRequestDto
    {
        [DefaultValue(false)]
        public bool Deleted { get; set; }
    }
}
