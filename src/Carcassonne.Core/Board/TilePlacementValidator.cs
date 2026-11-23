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
            if (!TryGetNeighborPosition(position, dx, dy, out BoardPosition neighborPosition) ||
                !board.TryGetValue(neighborPosition, out TileDefinition? neighbor))
                continue;

            hasNeighbor = true;
            if (tile.GetSide(side) != neighbor.GetSide(touchingSide))
                return new(TilePlacementError.SideMismatch,
                    $"Сторона не совпадает с плиткой в клетке ({neighborPosition.X}, {neighborPosition.Y}).");
        }

        if (!hasNeighbor)
            return new(TilePlacementError.NoAdjacentTile, "Плитка должна касаться соседней плитки стороной.");

        return new(TilePlacementError.None, null);
    }

    public static IReadOnlyList<TilePlacement> FindLegalPlacements(
        IReadOnlyDictionary<BoardPosition, TileDefinition> board,
        TileDefinition tile)
    {
        ArgumentNullException.ThrowIfNull(board);
        ArgumentNullException.ThrowIfNull(tile);

        var candidates = new HashSet<BoardPosition>();
        foreach (BoardPosition occupiedPosition in board.Keys)
        {
            foreach (var (_, _, dx, dy) in NeighborOffsets)
            {
                if (TryGetNeighborPosition(occupiedPosition, dx, dy, out BoardPosition candidate) &&
                    !board.ContainsKey(candidate))
                    candidates.Add(candidate);
            }
        }

        if (candidates.Count == 0)
            return [];

        var placements = new List<TilePlacement>();
        TileDefinition rotatedTile = tile;
        for (int degrees = 0; degrees < 360; degrees += 90)
        {
            if (degrees > 0)
                rotatedTile = rotatedTile.RotateClockwise();

            foreach (BoardPosition candidate in candidates)
            {
                if (Validate(board, candidate, rotatedTile).IsAllowed)
                    placements.Add(new TilePlacement(candidate, degrees));
            }
        }

        return placements.AsReadOnly();
    }

    private static bool TryGetNeighborPosition(BoardPosition position, int dx, int dy, out BoardPosition neighbor)
    {
        long x = (long)position.X + dx;
        long y = (long)position.Y + dy;
        if (x is < int.MinValue or > int.MaxValue || y is < int.MinValue or > int.MaxValue)
        {
            neighbor = default;
            return false;
        }

        neighbor = new BoardPosition((int)x, (int)y);
        return true;
    }
}
