namespace FireBreath.PostsMicroservice.Models.Dtos.ResponseDto
{
    public class HashtagResponseDto
    {
        public string Text { get; set; } = string.Empty;
        public int Count { get; set; } = 0;

        public HashtagResponseDto(string text, int count)
        {
            Text = text;
            Count = count;
        }
    }
}
