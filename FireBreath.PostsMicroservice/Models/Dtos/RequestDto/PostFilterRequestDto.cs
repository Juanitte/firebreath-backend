using Common.Dtos;
using Common.Utilities;

namespace FireBreath.PostsMicroservice.Models.Dtos.RequestDto
{
    public class PostFilterRequestDto : GenericFilterRequestDto
    {
        public bool ByDate { get; set; }
    }
}
