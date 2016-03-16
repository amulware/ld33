using Bearded.Utilities.SpaceTime;

namespace Centipede.Game
{
    interface IGameEvent
    {
        Position2 Position { get; }
        Unit VisibleRadius { get; }
        Unit AudibleRadius { get; }
        Unit MaxRadius { get; }

        void Send(GameState game);
    }
}