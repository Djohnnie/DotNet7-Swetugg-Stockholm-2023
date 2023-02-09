using DotNet7AndCSharp11.Orleans;
using DotNet7AndCSharp11.Orleans.Helpers;
using DotNet7AndCSharp11.Orleans.Hubs;
using DotNet7AndCSharp11.Orleans.Managers;
using DotNet7AndCSharp11.Orleans.Workers;
using DotNet7AndCSharp11.OrleansContracts;
using Microsoft.AspNetCore.Mvc;
using Orleans.Configuration;
using System.Net;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddEnvironmentVariables();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddMemoryCache();

builder.Services.AddSingleton<GameCodeHelper>();
builder.Services.AddTransient<GameManager>();
builder.Services.AddTransient(typeof(IApiHelper<>), typeof(ApiHelper<>));
builder.Services.AddSignalR();
builder.Services.AddSingleton<TickerHub>();
builder.Services.AddHostedService<TickerWorker>();

builder.Host.UseOrleans((hostBuilder, siloBuilder) =>
{
    siloBuilder.UseLocalhostClustering(siloPort: 11112, gatewayPort: 30001, primarySiloEndpoint: new IPEndPoint(IPAddress.Loopback, 11112), serviceId: "csharpwars-orleans-host", clusterId: "csharpwars-orleans-host");

    siloBuilder.Configure<ClusterOptions>(options =>
    {
        options.ClusterId = "csharpwars-orleans-host";
        options.ServiceId = "csharpwars-orleans-host";
    });

    siloBuilder.ConfigureLogging(loggingBuilder =>
    {
        loggingBuilder.AddConsole();
    });

    siloBuilder.UseDashboard();
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

//app.MapGet("/status", (_) => Results.Ok);
app.MapPost("/games", (IApiHelper<GameManager> helper, CreateGameRequest request) => helper.Post(l => l.CreateGame(request)));
app.MapPost("/games/{gameCode}/join", (IApiHelper<GameManager> helper, [FromRoute] string gameCode, JoinGameRequest request) => helper.Post(l => l.JoinGame(request with { GameCode = gameCode })));
app.MapPost("/games/{gameCode}/ready/{playerName}", (IApiHelper<GameManager> helper, [FromRoute] string gameCode, [FromRoute] string playerName) => helper.Post(l => l.ReadyPlayer(new ReadyPlayerRequest(gameCode, playerName))));
app.MapPost("/games/{gameCode}/abandon/{playerName}", (IApiHelper<GameManager> helper, [FromRoute] string gameCode, [FromRoute] string playerName) => helper.Post(l => l.Abandon(new AbandonRequest(gameCode, playerName))));
app.MapGet("/games/active", (IApiHelper<GameManager> helper) => helper.Execute(l => l.GetActiveGames()));

app.MapHub<TickerHub>("/ticker");


await app.RunAsync();