using VanguardLeagueSystems.Domain.Enums;

namespace VanguardLeagueSystems.Domain.ValueObjects;

/// <summary>
/// A flexible container for sport-specific rules.
/// This object is serialized to JSON in the database.
/// </summary>
public class SportConfiguration
{
    // Universal settings
    public bool AllowsTies { get; set; }
    public int PointsForWin { get; set; } = 3;
    public int PointsForTie { get; set; } = 1;
    public int PointsForLoss { get; set; } = 0;

    // Structure settings (Sport Dependent)
    // E.g., Hockey has 3 Periods, Soccer has 2 Halves.
    public string SegmentName { get; set; } = "Period"; // e.g., "Half", "Quarter"
    public int NumberOfSegments { get; set; } = 3;
    public int SegmentDurationMinutes { get; set; } = 20;

    // Elo integration flag
    public bool IsEloTracked { get; set; } = true;
}