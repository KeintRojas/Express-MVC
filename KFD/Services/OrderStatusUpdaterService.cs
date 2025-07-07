
using KFD.Data;
using Microsoft.EntityFrameworkCore;

namespace KFD.Services
{
    public class OrderStatusUpdaterService : BackgroundService
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly TimeSpan _interval = TimeSpan.FromSeconds(30);

        public OrderStatusUpdaterService(IServiceScopeFactory scopeFactory)
        {
            _scopeFactory = scopeFactory;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                using var scope = _scopeFactory.CreateScope();
                var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

                var now = DateTime.UtcNow;
                var orders = await context.orders.ToListAsync(stoppingToken);

                foreach (var order in orders) { 
                    var elapsed = now - order.Date;

                    string newState;
                    if (elapsed.TotalMinutes < 3)
                    {
                        newState = "Pendiente";
                    }else if (elapsed.TotalMinutes < 8)
                    {
                        newState = "Preparando";
                    }else if (elapsed.TotalMinutes < 15)
                    {
                        newState = "En camino";
                    }
                    else
                    {
                        newState = "Entregado";
                    }
                    if (order.State != newState)
                    {
                        order.State = newState;
                    }
                }
                await context.SaveChangesAsync(stoppingToken);

                await Task.Delay(_interval, stoppingToken);
            }
        }
    }
}
