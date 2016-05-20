
using Centipede.Game.AI;

namespace Centipede
{
    interface IBehaviour
    {
        void Start(CivilianController controller);
        void Update();
        void Stop();
    }
}

