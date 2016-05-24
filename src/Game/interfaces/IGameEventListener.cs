namespace Centipede.Game
{
    interface IGameEventListener : IPositionable
    {
        bool TryPerceive(GameEvent @event);
    }
}