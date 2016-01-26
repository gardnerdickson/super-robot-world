using System;
using RobotGame.Engine;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Input;

namespace RobotGame.Game.Mover
{
    class BasicMover : AbstractMover
    {
        // Constants ---------------------------------------------------------------------------------------- Contants

        // Data Members --------------------------------------------------------------------------------- Data Members

        private TimerCallback StoppedCallback;

        // Properties ------------------------------------------------------------------------------------- Properties

        // Contructors ----------------------------------------------------------------------------------- Contructors

        public BasicMover(Vector2 position, SpriteKey spriteKey, float acceleration, float maxVelocity,
                     float stopTime, string iterationMode, params MapWaypoint[] waypoints)
            : base(position, spriteKey, acceleration, maxVelocity, stopTime, iterationMode, waypoints)
        {
            this.StoppedCallback = new TimerCallback(stopped_disable);

            this.Start();
        }

        // Public Methods ----------------------------------------------------------------------------- Public Methods

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        // Protected Methods ----------------------------------------------------------------------- Protected Methods

        protected override void OnWaypointReached()
        {
            this.Stop();
            TimerManager.GetInstance().RegisterTimer(this.stopTime, StoppedCallback, null);

            base.OnWaypointReached();
        }

        // Private Methods --------------------------------------------------------------------------- Private Methods

        private void stopped_disable(Object param)
        {
            this.Start();
        }
    }
}
