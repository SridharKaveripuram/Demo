using Common;
using Microsoft.AspNetCore.SignalR;

namespace EmpdataConsumer
{
    public class EmployeeHub : Hub
    {
        private readonly ILogger<EmployeeHub> _logger;

        public EmployeeHub(ILogger<EmployeeHub> logger) { _logger = logger; }

        public override Task OnConnectedAsync()
        {
            _logger.LogInformation("Client Connected");
            return base.OnConnectedAsync();
        }

        public override Task OnDisconnectedAsync(Exception? exception)
        {
            _logger.LogInformation("Client Disconnected");
            return base.OnDisconnectedAsync(exception);
        }

        public async Task SendMessage(Employee employeeData)
        {
            if (employeeData != null && Clients?.All != null)
            {
                await Clients.All.SendAsync("RecieveMessage", employeeData);
            }
        }
    }
}
