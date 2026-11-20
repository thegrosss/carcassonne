using Carcassonne.Core.Tiles;
using Xunit;

namespace Carcassonne.Tests.Tiles;

public class TileDefinitionTests
{
    [Fact]
    public void SplitCitiesHaveSeparateIdsAndExits()
    {
        TileDefinition tile = SampleTiles.SplitCities;
        var cities = tile.Features.Where(feature => feature.Type == TileFeatureType.City);

        TileFeature northCity = Assert.Single(cities, city => city.Exits.Contains(new(TileSide.North)));
        TileFeature southCity = Assert.Single(cities, city => city.Exits.Contains(new(TileSide.South)));

        Assert.NotEqual(northCity.Id, southCity.Id);
        Assert.Single(northCity.Exits);
        Assert.Single(southCity.Exits);
    }

    [Fact]
    public void RoadAndBothFieldsCanReachTheSameSide()
    {
        TileDefinition tile = SampleTiles.StraightRoad;
        var center = new FeatureExit(TileSide.North);
        var left = new FeatureExit(TileSide.North, EdgeSlot.BeforeCenter);
        var right = new FeatureExit(TileSide.North, EdgeSlot.AfterCenter);

        Assert.Equal(TileTerrain.Road, tile.GetSide(TileSide.North));
        Assert.Equal(TileFeatureType.Road, Assert.Single(tile.Features, feature => feature.Exits.Contains(center)).Type);

        TileFeature leftField = Assert.Single(tile.Features, feature => feature.Exits.Contains(left));
        TileFeature rightField = Assert.Single(tile.Features, feature => feature.Exits.Contains(right));
        Assert.Equal(TileFeatureType.Field, leftField.Type);
        Assert.Equal(TileFeatureType.Field, rightField.Type);
        Assert.NotEqual(leftField.Id, rightField.Id);
    }

    [Fact]
    public void TileRejectsRepeatedFeatureIds()
    {
        var first = new TileFeature("same-id", TileFeatureType.Monastery);
        var second = new TileFeature("same-id", TileFeatureType.Field);

        Assert.Throws<ArgumentException>(() => new TileDefinition(
            "test-tile", TileTerrain.Field, TileTerrain.Field, TileTerrain.Field, TileTerrain.Field,
            first, second));
    }

    [Fact]
    public void TileRejectsAnExitSharedByTwoFeatures()
    {
        var exit = new FeatureExit(TileSide.North);
        var first = new TileFeature("road-a", TileFeatureType.Road, exit);
        var second = new TileFeature("road-b", TileFeatureType.Road, exit);

        Assert.Throws<ArgumentException>(() => new TileDefinition(
            "test-tile", TileTerrain.Road, TileTerrain.Field, TileTerrain.Field, TileTerrain.Field,
            first, second));
    }
}
