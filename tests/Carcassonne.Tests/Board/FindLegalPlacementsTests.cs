using Carcassonne.Core.Board;
using Carcassonne.Core.Tiles;
using Xunit;

namespace Carcassonne.Tests.Board;

public class FindLegalPlacementsTests
{
    [Fact]
    public void FindsTheMatchingRotationsForEachNeighboringCell()
    {
        var board = new Dictionary<BoardPosition, TileDefinition> { [new(0, 0)] = SampleTiles.Monastery };

        IReadOnlyList<TilePlacement> placements = TilePlacementValidator.FindLegalPlacements(board, SampleTiles.RoadEnd);

        TilePlacement[] expected =
        [
            new(new(0, -1), 90), new(new(0, -1), 180), new(new(0, -1), 270),
            new(new(1, 0), 0), new(new(1, 0), 180), new(new(1, 0), 270),
            new(new(0, 1), 0), new(new(0, 1), 90), new(new(0, 1), 270),
            new(new(-1, 0), 0), new(new(-1, 0), 90), new(new(-1, 0), 180)
        ];

        Assert.Equal(expected.Length, placements.Count);
        foreach (TilePlacement placement in expected)
            Assert.Contains(placement, placements);
        Assert.Single(board);
    }

    [Fact]
    public void ReturnsEachRotationOnceWhenACellTouchesTwoTiles()
    {
        var board = new Dictionary<BoardPosition, TileDefinition>
        {
            [new(0, -1)] = SampleTiles.Monastery,
            [new(1, -1)] = SampleTiles.Monastery,
            [new(1, 0)] = SampleTiles.Monastery
        };
        var sharedCell = new BoardPosition(0, 0);

        IReadOnlyList<TilePlacement> placements = TilePlacementValidator.FindLegalPlacements(board, SampleTiles.Monastery);

        int[] rotations = placements.Where(placement => placement.Position == sharedCell)
            .Select(placement => placement.RotationDegrees).Order().ToArray();
        Assert.Equal(new[] { 0, 90, 180, 270 }, rotations);
    }

    [Fact]
    public void ExcludesAPlacementThatCannotMatchBothNeighbors()
    {
        var board = new Dictionary<BoardPosition, TileDefinition>
        {
            [new(0, -1)] = SampleTiles.RoadEnd,
            [new(1, -1)] = SampleTiles.Monastery,
            [new(1, 0)] = SampleTiles.StraightRoad.RotateClockwise()
        };

        IReadOnlyList<TilePlacement> placements = TilePlacementValidator.FindLegalPlacements(board, SampleTiles.RoadEnd);

        Assert.DoesNotContain(placements, placement => placement.Position == new BoardPosition(0, 0));
        Assert.Contains(new TilePlacement(new(2, 0), 90), placements);
    }

    [Fact]
    public void ReturnsNoPlacementsWhenTheTileHasNoMatchingSide()
    {
        var board = new Dictionary<BoardPosition, TileDefinition> { [new(0, 0)] = SampleTiles.Monastery };
        var city = new TileFeature("city", TileFeatureType.City,
            new FeatureExit(TileSide.North), new FeatureExit(TileSide.East),
            new FeatureExit(TileSide.South), new FeatureExit(TileSide.West));
        var tile = new TileDefinition("test-all-city", TileTerrain.City, TileTerrain.City, TileTerrain.City, TileTerrain.City, city);

        Assert.Empty(TilePlacementValidator.FindLegalPlacements(board, tile));
    }

    [Fact]
    public void ReturnsNoPlacementsBeforeTheStartingTileIsOnTheBoard()
    {
        var board = new Dictionary<BoardPosition, TileDefinition>();

        Assert.Empty(TilePlacementValidator.FindLegalPlacements(board, SampleTiles.RoadEnd));
    }
}
