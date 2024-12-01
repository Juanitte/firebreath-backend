using Common.Dtos;
using Common.Utilities;

namespace FireBreath.PostsMicroservice.Models.Dtos.RequestDto
{
    public class PostFilterRequestDto : GenericFilterRequestDto
    {
        public int UserId { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
    }
}
