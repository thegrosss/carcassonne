namespace Carcassonne.Core.Board;

// Угол задан по часовой стрелке относительно переданной в поиск плитки.
public readonly record struct TilePlacement(BoardPosition Position, int RotationDegrees);
