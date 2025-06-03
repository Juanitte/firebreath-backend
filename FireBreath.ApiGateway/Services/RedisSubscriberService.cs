using Common.Utilities;
using FireBreath.ApiGateway.Notifications;
using Microsoft.AspNetCore.SignalR;
using StackExchange.Redis;

namespace FireBreath.ApiGateway.Services
{
    public class RedisSubscriberService : BackgroundService
    {
        private readonly IConnectionMultiplexer _redis;
        private readonly IHubContext<NotificationHub> _hubContext;

        public RedisSubscriberService(IConnectionMultiplexer redis, IHubContext<NotificationHub> hubContext)
        {
            _redis = redis;
            _hubContext = hubContext;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var subscriber = _redis.GetSubscriber();
            await subscriber.SubscribeAsync(new RedisChannel(Literals.Redis_New_Post_Signal, RedisChannel.PatternMode.Literal), async (channel, message) =>
            {
                await _hubContext.Clients.All.SendAsync("ReceivePost", message.ToString());
            });
        }
    }
}
