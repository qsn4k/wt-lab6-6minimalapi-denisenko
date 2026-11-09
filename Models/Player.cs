using System.ComponentModel.DataAnnotations;

namespace wt_lab6_6minimalapi_denisenko.Models;

/// <summary>Участник киберспортивного сервиса Tournament Hub.</summary>
public sealed class Player
{
    public int Id { get; set; }

    public string Nickname { get; set; } = string.Empty;

    public string Game { get; set; } = string.Empty;

    public int Rating { get; set; }

    public int Wins { get; set; }

    public int Losses { get; set; }

    public DateTime CreatedAt { get; set; }

    /// <summary>Процент побед, вычисляемый по числу побед и поражений.</summary>
    public double WinRate => Wins + Losses == 0 ? 0 : Math.Round(Wins * 100.0 / (Wins + Losses), 2);
}
