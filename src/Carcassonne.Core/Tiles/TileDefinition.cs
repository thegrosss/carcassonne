namespace Carcassonne.Core.Tiles;

public sealed class TileDefinition
{
    private readonly TileTerrain[] sides;

    public string Id { get; }
    public IReadOnlyList<TileFeature> Features { get; }

    public TileDefinition(
        string id,
        TileTerrain north,
        TileTerrain east,
        TileTerrain south,
        TileTerrain west,
        params TileFeature[] features)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(id);
        ArgumentNullException.ThrowIfNull(features);

        var featureIds = new HashSet<string>(StringComparer.Ordinal);
        var exits = new HashSet<FeatureExit>();

        foreach (TileFeature feature in features)
        {
            ArgumentNullException.ThrowIfNull(feature);

            if (!featureIds.Add(feature.Id))
                throw new ArgumentException($"Участок '{feature.Id}' указан несколько раз.", nameof(features));

            foreach (FeatureExit exit in feature.Exits)
            {
                if (!exits.Add(exit))
                    throw new ArgumentException($"Выход '{exit}' указан несколько раз.", nameof(features));
            }
        }

        Id = id;
        sides = [north, east, south, west];
        Features = Array.AsReadOnly(features.ToArray());
    }

    public TileTerrain GetSide(TileSide side)
    {
        int index = (int)side;
        if (index < 0 || index >= sides.Length)
            throw new ArgumentOutOfRangeException(nameof(side));

        return sides[index];
    }
}
