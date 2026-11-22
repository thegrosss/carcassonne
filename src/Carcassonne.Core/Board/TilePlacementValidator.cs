using Carcassonne.Core.Tiles;

namespace Carcassonne.Core.Board;

public static class TilePlacementValidator
{
    private static readonly (TileSide Side, TileSide TouchingSide, int Dx, int Dy)[] NeighborOffsets =
    [
        (TileSide.North, TileSide.South, 0, -1),
        (TileSide.East, TileSide.West, 1, 0),
        (TileSide.South, TileSide.North, 0, 1),
        (TileSide.West, TileSide.East, -1, 0)
    ];

    // Плитка передаётся в той ориентации, в которой игрок хочет её поставить.
    public static TilePlacementResult Validate(
        IReadOnlyDictionary<BoardPosition, TileDefinition> board,
        BoardPosition position,
        TileDefinition tile)
    {
        ArgumentNullException.ThrowIfNull(board);
        ArgumentNullException.ThrowIfNull(tile);

        if (board.ContainsKey(position))
            return new(TilePlacementError.Occupied, "Эта клетка уже занята.");

        bool hasNeighbor = false;

        foreach (var (side, touchingSide, dx, dy) in NeighborOffsets)
        {
            long x = (long)position.X + dx;
            long y = (long)position.Y + dy;
            if (x is < int.MinValue or > int.MaxValue || y is < int.MinValue or > int.MaxValue)
                continue;

            var neighborPosition = new BoardPosition((int)x, (int)y);
            if (!board.TryGetValue(neighborPosition, out TileDefinition? neighbor))
                continue;

            hasNeighbor = true;
            if (tile.GetSide(side) != neighbor.GetSide(touchingSide))
                return new(TilePlacementError.SideMismatch, $"Сторона не совпадает с плиткой в клетке ({x}, {y}).");
        }

        if (!hasNeighbor)
            return new(TilePlacementError.NoAdjacentTile, "Плитка должна касаться соседней плитки стороной.");

        return new(TilePlacementError.None, null);
    }
}
