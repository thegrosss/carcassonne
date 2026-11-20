namespace Carcassonne.Core.Tiles;

// Стороны читаются по часовой стрелке: северная слева направо,
// восточная сверху вниз, южная справа налево, западная снизу вверх.
public enum EdgeSlot
{
    Center,
    BeforeCenter,
    AfterCenter
}

public readonly record struct FeatureExit(TileSide Side, EdgeSlot Slot = EdgeSlot.Center);
