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

    [Fact]
    public void ClockwiseRotationMovesRoadSideAndExitTogether()
    {
        TileDefinition original = SampleTiles.RoadEnd;

        TileDefinition rotated = original.RotateClockwise();

        Assert.Equal(TileTerrain.Road, rotated.GetSide(TileSide.West));
        Assert.Equal(TileTerrain.Field, rotated.GetSide(TileSide.South));
        TileFeature road = Assert.Single(rotated.Features, feature => feature.Type == TileFeatureType.Road);
        Assert.Equal(new FeatureExit(TileSide.West), Assert.Single(road.Exits));

        Assert.Equal(TileTerrain.Road, original.GetSide(TileSide.South));
        TileFeature originalRoad = Assert.Single(original.Features, feature => feature.Type == TileFeatureType.Road);
        Assert.Equal(new FeatureExit(TileSide.South), Assert.Single(originalRoad.Exits));
    }

    [Fact]
    public void ClockwiseRotationKeepsFieldsOnTheirSidesOfTheRoad()
    {
        TileDefinition rotated = SampleTiles.StraightRoad.RotateClockwise();
        var beforeCenter = new FeatureExit(TileSide.East, EdgeSlot.BeforeCenter);
        var afterCenter = new FeatureExit(TileSide.East, EdgeSlot.AfterCenter);

        TileFeature firstField = Assert.Single(rotated.Features, feature => feature.Exits.Contains(beforeCenter));
        TileFeature secondField = Assert.Single(rotated.Features, feature => feature.Exits.Contains(afterCenter));

        Assert.Equal("west-field", firstField.Id);
        Assert.Equal("east-field", secondField.Id);
    }

    [Fact]
    public void FourClockwiseRotationsRestoreSidesAndFeatures()
    {
        TileDefinition[] tiles = [SampleTiles.RoadEnd, SampleTiles.StraightRoad, SampleTiles.SplitCities, SampleTiles.Monastery];

        foreach (TileDefinition original in tiles)
        {
            TileDefinition rotated = original;
            for (int i = 0; i < 4; i++)
                rotated = rotated.RotateClockwise();

            Assert.Equal(original.Id, rotated.Id);
            foreach (TileSide side in Enum.GetValues<TileSide>())
                Assert.Equal(original.GetSide(side), rotated.GetSide(side));

            Assert.Equal(original.Features.Count, rotated.Features.Count);
            for (int i = 0; i < original.Features.Count; i++)
            {
                TileFeature originalFeature = original.Features[i];
                TileFeature rotatedFeature = rotated.Features[i];
                Assert.Equal(originalFeature.Id, rotatedFeature.Id);
                Assert.Equal(originalFeature.Type, rotatedFeature.Type);
                Assert.Equal<FeatureExit>(originalFeature.Exits, rotatedFeature.Exits);
            }
        }
    }
}
