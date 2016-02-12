using Bearded.Utilities.SpaceTime;

namespace Centipede.Game
{
    interface IProjectileCollider
    {
        Position2 Center { get; }
        Unit Radius { get; }
    }
}