namespace Centipede.Game
{
    interface IGameEventListener : IPositionable
    {
        bool TryPerceive(IGameEvent @event);
    }
}