namespace Carcassonne.Core.Tiles;

public enum TileFeatureType
{
    Field,
    Road,
    City,
    Monastery
}

public sealed class TileFeature
{
    // Идентификатор участка уникален в пределах одной плитки.
    public string Id { get; }
    public TileFeatureType Type { get; }
    public IReadOnlyList<FeatureExit> Exits { get; }

    public TileFeature(string id, TileFeatureType type, params FeatureExit[] exits)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(id);
        ArgumentNullException.ThrowIfNull(exits);

        Id = id;
        Type = type;
        Exits = Array.AsReadOnly(exits.ToArray());
    }
}
