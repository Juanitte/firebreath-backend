using Newtonsoft.Json;

namespace FireBreath.PostsMicroservice.Services
{
    public interface IUserFollowService
    {
        Task<List<int>> GetFollowedUserIdsAsync(int userId);
    }
    public class UserFollowService : BaseService, IUserFollowService
    {
        #region Miembros privados

        private readonly HttpClient _httpClient;

        #endregion

        #region Constructores

        public UserFollowService(IHttpClientFactory factory, ILogger logger, HttpClient httpClient) : base(logger)
        {
            _httpClient = factory.CreateClient("users-ms");
        }

        #endregion

        #region Implementación de IUserFollowService

        public async Task<List<int>> GetFollowedUserIdsAsync(int userId)
        {
            try
            {
                var response = await _httpClient.GetAsync($"/users/followingIds?userId={userId}");
                response.EnsureSuccessStatusCode();

                var json = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<List<int>>(json) ?? new List<int>();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener followingIds desde users-ms");
                return new List<int>();
            }
        }

        #endregion
    }
}
