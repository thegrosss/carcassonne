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

    public TileDefinition RotateClockwise()
    {
        var rotatedFeatures = new TileFeature[Features.Count];

        for (int i = 0; i < Features.Count; i++)
        {
            TileFeature feature = Features[i];
            var rotatedExits = new FeatureExit[feature.Exits.Count];

            for (int j = 0; j < feature.Exits.Count; j++)
            {
                FeatureExit exit = feature.Exits[j];
                // Slot отсчитывается по часовой стрелке вдоль стороны и при повороте сохраняется.
                rotatedExits[j] = new FeatureExit(RotateSideClockwise(exit.Side), exit.Slot);
            }

            rotatedFeatures[i] = new TileFeature(feature.Id, feature.Type, rotatedExits);
        }

        return new TileDefinition(
            Id,
            north: GetSide(TileSide.West),
            east: GetSide(TileSide.North),
            south: GetSide(TileSide.East),
            west: GetSide(TileSide.South),
            features: rotatedFeatures);
    }

    private static TileSide RotateSideClockwise(TileSide side) => side switch
    {
        TileSide.North => TileSide.East,
        TileSide.East => TileSide.South,
        TileSide.South => TileSide.West,
        TileSide.West => TileSide.North,
        _ => throw new ArgumentOutOfRangeException(nameof(side))
    };
}
