using wt_lab6_6minimalapi_denisenko.Models;

namespace wt_lab6_6minimalapi_denisenko.Data;

/// <summary>
/// Единое временное хранилище всех API-операций.
/// Данные находятся в памяти процесса и сбрасываются при перезапуске приложения.
/// </summary>
public static class PlayerStore
{
    public static List<Player> Players { get; } = new()
    {
        new Player { Id = 1, Nickname = "CyberWolf",   Game = "Counter-Strike 2", Rating = 1500, Wins = 24, Losses = 12, CreatedAt = new DateTime(2026, 9, 1, 0, 0, 0, DateTimeKind.Utc) },
        new Player { Id = 2, Nickname = "ShadowFox",   Game = "Counter-Strike 2", Rating = 1750, Wins = 31, Losses = 10, CreatedAt = new DateTime(2026, 9, 2, 0, 0, 0, DateTimeKind.Utc) },
        new Player { Id = 3, Nickname = "NeoMaster",   Game = "Dota 2",           Rating = 2100, Wins = 38, Losses = 9,  CreatedAt = new DateTime(2026, 9, 3, 0, 0, 0, DateTimeKind.Utc) },
        new Player { Id = 4, Nickname = "DarkKnight",  Game = "Dota 2",           Rating = 1850, Wins = 29, Losses = 15, CreatedAt = new DateTime(2026, 9, 4, 0, 0, 0, DateTimeKind.Utc) },
        new Player { Id = 5, Nickname = "PixelHunter", Game = "Valorant",         Rating = 1600, Wins = 21, Losses = 18, CreatedAt = new DateTime(2026, 9, 5, 0, 0, 0, DateTimeKind.Utc) },
        new Player { Id = 6, Nickname = "IceStorm",    Game = "Valorant",         Rating = 1900, Wins = 34, Losses = 11, CreatedAt = new DateTime(2026, 9, 6, 0, 0, 0, DateTimeKind.Utc) },
        new Player { Id = 7, Nickname = "RedDragon",   Game = "Counter-Strike 2", Rating = 1250, Wins = 15, Losses = 20, CreatedAt = new DateTime(2026, 9, 7, 0, 0, 0, DateTimeKind.Utc) }
    };
}
