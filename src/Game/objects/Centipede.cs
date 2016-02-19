using System.Collections.Generic;
using Bearded.Utilities.SpaceTime;
using Centipede.Game.CentipedeParts;

namespace Centipede.Game
{
    sealed class Centipede : GameObject, IPositionable
    {
        private readonly CentiHead head;
        private readonly List<Centipart> parts = new List<Centipart>();

        private readonly LinkedList<CentiPathPart> tailPath = new LinkedList<CentiPathPart>();

        private readonly KeyboardController controller = new KeyboardController();

        private CentiPathPart lastSaved;

        public Centipede(GameState game, int length = 15)
            : base(game)
        {
            this.head = new CentiHead(game);

            this.tailPath.AddFirst(new CentiPathPart(this.head.Position, 0.U()));

            for (int i = 0; i < length; i++)
            {
                this.parts.Add(new Centipart(game));
            }
        }

        public Position2 Position { get { return this.head.Position; } }

        public override void Update(TimeSpan elapsedTime)
        {
            var controlState = this.controller.Control();
            this.head.Update(this.game.Time, elapsedTime, controlState);

            var currentPathPart = new CentiPathPart(this.head.Position, this.lastSaved);

            this.updateParts(currentPathPart);

            if (currentPathPart.DistanceToPrevious > 0.5f.U())
            {
                this.tailPath.AddFirst(currentPathPart);
                this.lastSaved = currentPathPart;
            }
        }

        private void updateParts(CentiPathPart startPosition)
        {
            var intervalStart = startPosition;
            var intervalStartDistance = 0.U();

            var intervalLength = intervalStart.DistanceToPrevious;

            var intervalEndNode = this.tailPath.First;


            var i = 0;

            var partDistanceStep = 1.5f.U();
            var partDistance = partDistanceStep;

            while (true)
            {
                var intervalEnd = intervalEndNode.Value;
                var intervalEndDistance = intervalStartDistance + intervalLength;

                if (intervalLength > 0.U())
                {
                    while (partDistance < intervalEndDistance)
                    {
                        // position part, move to next
                        var part = this.parts[i];

                        var p = (partDistance - intervalStartDistance) / intervalLength;

                        part.SetPosition(intervalStart.Position.LerpTo(intervalEnd.Position, p));

                        partDistance += partDistanceStep;
                        i++;

                        if (i == this.parts.Count)
                        {
                            while (this.tailPath.Last != intervalEndNode)
                                this.tailPath.RemoveLast();
                            
                            return;
                        }
                    }
                }


                if (intervalEndNode.Next == null)
                {
                    for (; i < this.parts.Count; i++)
                    {
                        var part = this.parts[i];
                        part.SetPosition(intervalEnd.Position);
                    }
                    return;
                }

                intervalStart = intervalEnd;
                intervalStartDistance = intervalEndDistance;
                intervalLength = intervalStart.DistanceToPrevious;

                intervalEndNode = intervalEndNode.Next;
            }
        }

        public override void Draw()
        {
            this.head.Draw();
            foreach (var part in this.parts)
            {
                part.Draw();
            }

            // debug draw tail path
#if false

            var p = this.head.Position.Vector;
            foreach (var part in this.tailPath)
            {
                var p2 = part.Position.Vector;

                geo.Color = Color.Green;
                geo.LineWidth = 0.05f;
                geo.DrawLine(p, p2);

                p = p2;
            }
#endif
        }

        protected override void onDelete()
        {
            this.head.Delete();
            foreach (var part in this.parts)
            {
                part.Delete();
            }
        }
    }
}