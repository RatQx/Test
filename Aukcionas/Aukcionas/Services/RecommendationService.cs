using Aukcionas.Data;
using Aukcionas.Utils;

namespace Aukcionas.Services
{
    public class RecommendationService : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly TimeSpan _checkInterval = TimeSpan.FromMinutes(1);

        public RecommendationService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                using (var scope = _serviceProvider.CreateScope())
                {
                    var dbContext = scope.ServiceProvider.GetRequiredService<DataContext>();
                    var auctionRecommender = scope.ServiceProvider.GetRequiredService<AuctionRecommender>();
                    await auctionRecommender.TrainModelsForUsers(dbContext);
                }

                await Task.Delay(_checkInterval, stoppingToken);
            }
        }
    }
}
