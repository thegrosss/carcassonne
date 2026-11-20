namespace Carcassonne.Core.Tiles;

public static class SampleTiles
{
    public static TileDefinition StraightRoad { get; } = new(
        "sample-straight-road",
        TileTerrain.Road, TileTerrain.Field, TileTerrain.Road, TileTerrain.Field,
        new TileFeature("road", TileFeatureType.Road,
            new FeatureExit(TileSide.North), new FeatureExit(TileSide.South)),
        new TileFeature("west-field", TileFeatureType.Field,
            new FeatureExit(TileSide.North, EdgeSlot.BeforeCenter),
            new FeatureExit(TileSide.South, EdgeSlot.AfterCenter),
            new FeatureExit(TileSide.West, EdgeSlot.BeforeCenter),
            new FeatureExit(TileSide.West, EdgeSlot.AfterCenter)),
        new TileFeature("east-field", TileFeatureType.Field,
            new FeatureExit(TileSide.North, EdgeSlot.AfterCenter),
            new FeatureExit(TileSide.East, EdgeSlot.BeforeCenter),
            new FeatureExit(TileSide.East, EdgeSlot.AfterCenter),
            new FeatureExit(TileSide.South, EdgeSlot.BeforeCenter)));

    public static TileDefinition RoadEnd { get; } = new(
        "sample-road-end",
        TileTerrain.Field, TileTerrain.Field, TileTerrain.Road, TileTerrain.Field,
        new TileFeature("road", TileFeatureType.Road, new FeatureExit(TileSide.South)),
        new TileFeature("field", TileFeatureType.Field,
            new FeatureExit(TileSide.North, EdgeSlot.BeforeCenter),
            new FeatureExit(TileSide.North, EdgeSlot.AfterCenter),
            new FeatureExit(TileSide.East, EdgeSlot.BeforeCenter),
            new FeatureExit(TileSide.East, EdgeSlot.AfterCenter),
            new FeatureExit(TileSide.South, EdgeSlot.BeforeCenter),
            new FeatureExit(TileSide.South, EdgeSlot.AfterCenter),
            new FeatureExit(TileSide.West, EdgeSlot.BeforeCenter),
            new FeatureExit(TileSide.West, EdgeSlot.AfterCenter)));

    public static TileDefinition SplitCities { get; } = new(
        "sample-split-cities",
        TileTerrain.City, TileTerrain.Field, TileTerrain.City, TileTerrain.Field,
        new TileFeature("north-city", TileFeatureType.City, new FeatureExit(TileSide.North)),
        new TileFeature("south-city", TileFeatureType.City, new FeatureExit(TileSide.South)),
        new TileFeature("field", TileFeatureType.Field,
            new FeatureExit(TileSide.East, EdgeSlot.BeforeCenter),
            new FeatureExit(TileSide.East, EdgeSlot.AfterCenter),
            new FeatureExit(TileSide.West, EdgeSlot.BeforeCenter),
            new FeatureExit(TileSide.West, EdgeSlot.AfterCenter)));

    public static TileDefinition Monastery { get; } = new(
        "sample-monastery",
        TileTerrain.Field, TileTerrain.Field, TileTerrain.Field, TileTerrain.Field,
        new TileFeature("monastery", TileFeatureType.Monastery),
        new TileFeature("field", TileFeatureType.Field,
            new FeatureExit(TileSide.North, EdgeSlot.BeforeCenter),
            new FeatureExit(TileSide.North, EdgeSlot.AfterCenter),
            new FeatureExit(TileSide.East, EdgeSlot.BeforeCenter),
            new FeatureExit(TileSide.East, EdgeSlot.AfterCenter),
            new FeatureExit(TileSide.South, EdgeSlot.BeforeCenter),
            new FeatureExit(TileSide.South, EdgeSlot.AfterCenter),
            new FeatureExit(TileSide.West, EdgeSlot.BeforeCenter),
            new FeatureExit(TileSide.West, EdgeSlot.AfterCenter)));
}
