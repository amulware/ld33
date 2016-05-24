using Bearded.Utilities.Math;
using Bearded.Utilities.SpaceTime;

namespace Centipede.Game
{
    interface IGameEvent
    {
        Position2 Position { get; }
        Unit VisibleRadius { get; }
        Unit AudibleRadius { get; }
        Unit MaxRadius { get; }
        int PerceivedBy { get; }

        void Send(GameState game);

        bool CanBePerceivedAt(Position2 position);
        bool CanBePerceivedAt(Position2 position, Direction2 visionDirection, Angle visionHalfAngle);

        void PerceiveBy(IGameEventPerceiver perceiver);
    }
}