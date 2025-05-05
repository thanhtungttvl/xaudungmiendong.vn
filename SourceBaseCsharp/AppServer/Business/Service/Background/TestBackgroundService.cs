namespace AppServer.Business.Service.Background
{
    public class TestBackgroundService : BackgroundService
    {
        public TestBackgroundService()
        {

        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                await Task.Delay(1000, stoppingToken); // Cập nhật mỗi giây
            }
        }

    }
}
