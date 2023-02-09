using DotNet7AndCSharp11.Orleans.Grains;
using DotNet7AndCSharp11.OrleansContracts;
using Microsoft.AspNetCore.SignalR;
using GameState = DotNet7AndCSharp11.OrleansContracts.GameState;

namespace DotNet7AndCSharp11.Orleans.Hubs;

public class TickerHub : Hub
{
    private readonly IServiceScopeFactory _serviceScopeFactory;

    public TickerHub(IServiceScopeFactory serviceScopeFactory)
    {
        _serviceScopeFactory = serviceScopeFactory;
    }

    public async Task SendGameState(GameState gameState)
    {
        if (Clients != null)
        {
            await Clients.Group(gameState.GameCode).SendAsync("ReceiveGameState", gameState);
        }
    }

    public async Task JoinGame(string gameCode)
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, gameCode);
    }

    public async Task Turn(string gameCode, string playerName, Orientation orientation)
    {
        if (string.IsNullOrWhiteSpace(gameCode) || string.IsNullOrWhiteSpace(playerName))
        {
            return;
        }

        using var scope = _serviceScopeFactory.CreateScope();
        var grainFactory = scope.ServiceProvider.GetService<IGrainFactory>();

        var playerGrain = grainFactory.GetGrain<IPlayerGrain>(playerName);
        await playerGrain.SetOrientation(orientation);
    }
}