using Carcassonne.Core.Board;
using Carcassonne.Core.Tiles;
using Xunit;

namespace Carcassonne.Tests.Board;

public class TilePlacementValidatorTests
{
    [Fact]
    public void AllowsRotatedTileWhenBothNeighborsMatch()
    {
        TileDefinition westNeighbor = SampleTiles.RoadEnd.RotateClockwise().RotateClockwise().RotateClockwise();
        var board = new Dictionary<BoardPosition, TileDefinition>
        {
            [new(-1, -1)] = westNeighbor,
            [new(0, -2)] = SampleTiles.Monastery
        };

        var position = new BoardPosition(0, -1);
        TileDefinition tile = SampleTiles.RoadEnd.RotateClockwise();

        TilePlacementResult result = TilePlacementValidator.Validate(board, position, tile);

        Assert.True(result.IsAllowed);
        Assert.Null(result.Message);
        Assert.False(board.ContainsKey(position));
    }

    [Fact]
    public void RejectsOccupiedCell()
    {
        var position = new BoardPosition(0, 0);
        var board = new Dictionary<BoardPosition, TileDefinition> { [position] = SampleTiles.Monastery };

        TilePlacementResult result = TilePlacementValidator.Validate(board, position, SampleTiles.RoadEnd);

        Assert.False(result.IsAllowed);
        Assert.Equal(TilePlacementError.Occupied, result.Error);
        Assert.False(string.IsNullOrWhiteSpace(result.Message));
    }

    [Theory]
    [InlineData(3, 0)]
    [InlineData(1, 1)]
    public void RejectsTileWithoutNeighborOnAnySide(int x, int y)
    {
        var board = new Dictionary<BoardPosition, TileDefinition> { [new(0, 0)] = SampleTiles.Monastery };

        TilePlacementResult result = TilePlacementValidator.Validate(board, new(x, y), SampleTiles.RoadEnd);

        Assert.False(result.IsAllowed);
        Assert.Equal(TilePlacementError.NoAdjacentTile, result.Error);
        Assert.False(string.IsNullOrWhiteSpace(result.Message));
    }

    [Fact]
    public void RejectsMismatchEvenWhenAnotherNeighborMatches()
    {
        var board = new Dictionary<BoardPosition, TileDefinition>
        {
            [new(0, -1)] = SampleTiles.RoadEnd,
            [new(1, 0)] = SampleTiles.StraightRoad.RotateClockwise()
        };

        TilePlacementResult result = TilePlacementValidator.Validate(board, new(0, 0), SampleTiles.StraightRoad);

        Assert.False(result.IsAllowed);
        Assert.Equal(TilePlacementError.SideMismatch, result.Error);
        Assert.Contains("(1, 0)", result.Message);
    }

    [Fact]
    public void RejectsPlacementBeforeStartingTileIsOnTheBoard()
    {
        var board = new Dictionary<BoardPosition, TileDefinition>();

        TilePlacementResult result = TilePlacementValidator.Validate(board, new(0, 0), SampleTiles.Monastery);

        Assert.Equal(TilePlacementError.NoAdjacentTile, result.Error);
    }

    [Fact]
    public void CoordinatesAtOppositeLimitsAreNotNeighbors()
    {
        var board = new Dictionary<BoardPosition, TileDefinition> { [new(int.MinValue, 0)] = SampleTiles.Monastery };

        TilePlacementResult result = TilePlacementValidator.Validate(board, new(int.MaxValue, 0), SampleTiles.Monastery);

        Assert.Equal(TilePlacementError.NoAdjacentTile, result.Error);
    }
}
