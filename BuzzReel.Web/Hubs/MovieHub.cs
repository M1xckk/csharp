using Microsoft.AspNetCore.SignalR;

namespace BuzzReel.Web.Hubs;
public class MovieHub : Hub
{
    public override Task OnConnectedAsync()
    {
        var userId = Context.GetHttpContext().Request.Query["userId"];
        Console.WriteLine($"A Client Connected: {Context.ConnectionId} with userId: {userId}");
        return Groups.AddToGroupAsync(Context.ConnectionId, userId)
                     .ContinueWith(_ => base.OnConnectedAsync());
    }

    public override Task OnDisconnectedAsync(Exception exception)
    {
        var userId = Context.GetHttpContext().Request.Query["userId"];
        Console.WriteLine($"A Client Disconnected: {Context.ConnectionId} with userId: {userId}");
        return Groups.RemoveFromGroupAsync(Context.ConnectionId, userId)
                     .ContinueWith(_ => base.OnDisconnectedAsync(exception));
    }
}
