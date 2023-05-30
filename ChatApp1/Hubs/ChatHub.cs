using ChatApp1.Dto;
using Microsoft.AspNetCore.SignalR;
using System.Text.RegularExpressions;

namespace ChatApp1.Hubs
{
    public class ChatHub: Hub
    {
        public override Task OnConnectedAsync()
        {
            Groups.AddToGroupAsync(Context.ConnectionId, Context.User.Identity.Name);
            HttpContext httpContext = Context.GetHttpContext();

            string receiver = httpContext.Request.Query["userid"];
            string sender = Context.User.Claims.FirstOrDefault().Value;

            Groups.AddToGroupAsync(Context.ConnectionId, sender);
            if (!string.IsNullOrEmpty(receiver))
            {
                Groups.AddToGroupAsync(Context.ConnectionId, receiver);
            }

            return base.OnConnectedAsync();
        }
        public Task SendMessageToGroup(ChatDto chat)
        {
            return Clients.Group(chat.Reciever).SendAsync("ReceiveMessage"
                , Context.User.Identity.Name , chat.Message);
        }
    }
}
