using Common.Dtos;
using Common.Utilities;
using FireBreath.PostsMicroservice.Models.Dtos.EntityDto;
using FireBreath.PostsMicroservice.Models.Entities;

namespace FireBreath.PostsMicroservice.Models.Dtos.ResponseDto
{
    public class ResponseFilterPostDto : GenericFilterDto
    {
        public ResponseFilterPostDto()
        {
            this.Posts = new List<PostDto>();
            FilterablesFields = Extensions.GetFilterablesFields<Post>();
        }

        public List<PostDto> Posts { get; set; }
    }
}
