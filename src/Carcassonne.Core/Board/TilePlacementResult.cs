namespace Carcassonne.Core.Board;

public enum TilePlacementError
{
    None,
    Occupied,
    NoAdjacentTile,
    SideMismatch
}

public readonly record struct TilePlacementResult(TilePlacementError Error, string? Message)
{
    public bool IsAllowed => Error == TilePlacementError.None;
}
