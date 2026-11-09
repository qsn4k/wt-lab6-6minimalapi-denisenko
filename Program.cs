using System.ComponentModel.DataAnnotations;
using Microsoft.OpenApi;
using wt_lab6_6minimalapi_denisenko.Data;
using wt_lab6_6minimalapi_denisenko.Models;

var builder = WebApplication.CreateBuilder(args);

// Swashbuckle generates an OpenAPI document and hosts the interactive Swagger UI.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Tournament Hub REST API",
        Version = "v1",
        Description = "REST API для управления профилями игроков киберспортивного сервиса."
    });
});

var app = builder.Build();

// Swagger is intentionally enabled in every environment so it remains available on Render.
app.UseSwagger();
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "Tournament Hub API v1");
    options.RoutePrefix = "swagger";
    options.DocumentTitle = "Tournament Hub API — Swagger";
});

app.MapGet("/", () => Results.Text("Minimal API работает!", "text/plain; charset=utf-8"))
    .WithName("Root")
    .WithTags("Service")
    .WithSummary("Проверка доступности приложения")
    .Produces<string>(StatusCodes.Status200OK, "text/plain");

app.MapGet("/api/info", () => Results.Ok(new
    {
        application = "Tournament Hub",
        technology = "ASP.NET Core Minimal API",
        version = "1.0.0",
        description = "REST API для управления профилями игроков",
        documentation = "/swagger"
    }))
    .WithName("GetApiInfo")
    .WithTags("Service")
    .WithSummary("Получение сведений о приложении")
    .Produces(StatusCodes.Status200OK);

app.MapGet("/api/players", GetPlayers)
    .WithName("GetPlayers")
    .WithTags("Players")
    .WithSummary("Получить список игроков")
    .WithDescription("Поддерживает поиск по никнейму/игре и фильтрацию по точному названию игры.")
    .Produces<List<Player>>(StatusCodes.Status200OK)
    .ProducesValidationProblem();

app.MapGet("/api/players/{id:int}", GetPlayerById)
    .WithName("GetPlayerById")
    .WithTags("Players")
    .WithSummary("Получить профиль игрока по ID")
    .Produces<Player>(StatusCodes.Status200OK)
    .Produces(StatusCodes.Status404NotFound);

app.MapPost("/api/players", CreatePlayer)
    .WithName("CreatePlayer")
    .WithTags("Players")
    .WithSummary("Создать профиль игрока")
    .WithDescription("ID и дата регистрации назначаются сервером. Тело запроса передаётся в формате JSON.")
    .Accepts<PlayerRequest>("application/json")
    .Produces<Player>(StatusCodes.Status201Created)
    .ProducesValidationProblem();

app.MapPut("/api/players/{id:int}", UpdatePlayer)
    .WithName("UpdatePlayer")
    .WithTags("Players")
    .WithSummary("Изменить профиль игрока")
    .Accepts<PlayerRequest>("application/json")
    .Produces<Player>(StatusCodes.Status200OK)
    .Produces(StatusCodes.Status404NotFound)
    .ProducesValidationProblem();

app.MapDelete("/api/players/{id:int}", DeletePlayer)
    .WithName("DeletePlayer")
    .WithTags("Players")
    .WithSummary("Удалить профиль игрока")
    .Produces(StatusCodes.Status204NoContent)
    .Produces(StatusCodes.Status404NotFound);

static IResult GetPlayers(string? search, string? game)
{
    IEnumerable<Player> result = PlayerStore.Players;

    if (!string.IsNullOrWhiteSpace(search))
    {
        var term = search.Trim();
        result = result.Where(player =>
            player.Nickname.Contains(term, StringComparison.OrdinalIgnoreCase) ||
            player.Game.Contains(term, StringComparison.OrdinalIgnoreCase));
    }

    if (!string.IsNullOrWhiteSpace(game))
    {
        result = result.Where(player => player.Game.Equals(game.Trim(), StringComparison.OrdinalIgnoreCase));
    }

    return Results.Ok(result.OrderByDescending(player => player.Rating).ThenBy(player => player.Nickname).ToList());
}

static IResult GetPlayerById(int id)
{
    var player = PlayerStore.Players.FirstOrDefault(item => item.Id == id);
    return player is null ? Results.NotFound() : Results.Ok(player);
}

static IResult CreatePlayer(PlayerRequest request)
{
    var errors = ValidateRequest(request);
    if (errors.Count > 0)
    {
        return Results.ValidationProblem(errors);
    }

    var newId = PlayerStore.Players.Count == 0 ? 1 : PlayerStore.Players.Max(player => player.Id) + 1;
    var player = new Player
    {
        Id = newId,
        Nickname = request.Nickname.Trim(),
        Game = request.Game.Trim(),
        Rating = request.Rating,
        Wins = request.Wins,
        Losses = request.Losses,
        CreatedAt = DateTime.UtcNow
    };

    PlayerStore.Players.Add(player);
    return Results.Created($"/api/players/{player.Id}", player);
}

static IResult UpdatePlayer(int id, PlayerRequest request)
{
    var player = PlayerStore.Players.FirstOrDefault(item => item.Id == id);
    if (player is null)
    {
        return Results.NotFound();
    }

    var errors = ValidateRequest(request);
    if (errors.Count > 0)
    {
        return Results.ValidationProblem(errors);
    }

    player.Nickname = request.Nickname.Trim();
    player.Game = request.Game.Trim();
    player.Rating = request.Rating;
    player.Wins = request.Wins;
    player.Losses = request.Losses;

    return Results.Ok(player);
}

static IResult DeletePlayer(int id)
{
    var player = PlayerStore.Players.FirstOrDefault(item => item.Id == id);
    if (player is null)
    {
        return Results.NotFound();
    }

    PlayerStore.Players.Remove(player);
    return Results.NoContent();
}

static Dictionary<string, string[]> ValidateRequest(PlayerRequest request)
{
    var validationResults = new List<ValidationResult>();
    var context = new ValidationContext(request);
    Validator.TryValidateObject(request, context, validationResults, validateAllProperties: true);

    return validationResults
        .SelectMany(result => result.MemberNames.DefaultIfEmpty("request")
            .Select(member => new { Member = member, Message = result.ErrorMessage ?? "Некорректное значение." }))
        .GroupBy(item => item.Member)
        .ToDictionary(group => group.Key, group => group.Select(item => item.Message).Distinct().ToArray());
}

// Render injects PORT. The fallback makes Docker and local startup predictable.
var port = Environment.GetEnvironmentVariable("PORT");
if (string.IsNullOrWhiteSpace(port))
{
    port = "10000";
}
app.Run($"http://0.0.0.0:{port}");
