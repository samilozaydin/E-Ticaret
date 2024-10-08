using ETicaretAPI.Application.Abstractions.Hubs;
using ETicaretAPI.SignalR.Hubs;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETicaretAPI.SignalR.HubServices
{
    public class OrderHubService : IOrderHubService
    {
        readonly IHubContext<OrderHub> _orderHub;

        public OrderHubService(IHubContext<OrderHub> orderHub)
        {
            _orderHub = orderHub;
        }
        public async Task OrderAddedMessageAsync(string message)
        {
            await _orderHub.Clients.All.SendAsync(ReceiveFunctionNames.OrderAddedMessage,message);
        }
    }
}
