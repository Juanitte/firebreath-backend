using FireBreath.PostsMicroservice.Models.Dtos.CreateDto;
using FireBreath.PostsMicroservice.Models.Dtos.EntityDto;

namespace FireBreath.PostsMicroservice.Models.Dtos.ResponseDto
{
    public class CreateChatResponseDto : CreateEditRemoveResponseDto
    {
        public ChatDto Chat { get; set; } = new ChatDto();
    }
}
