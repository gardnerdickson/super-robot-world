using System;
using Microsoft.Xna.Framework;
using RobotGame.Engine;

namespace RobotGame.Game.Mover
{
    class SuspendedMover : AbstractMover
    {
        // Constants ---------------------------------------------------------------------------------------- Contants

        private const double RESET_DELAY = Config.SUSPENDED_MOVER_RESET_DELAY;

        // Data Members --------------------------------------------------------------------------------- Data Members

        private TimerCallback ResetCallback;

        private bool deactivated;
        private bool offScreenReset;

        // Properties ------------------------------------------------------------------------------------- Properties

        // Contructors ----------------------------------------------------------------------------------- Contructors

        public SuspendedMover(Vector2 position, SpriteKey spriteKey, float acceleration,
                              float maxVelocity, float stopTime, bool offScreenReset, params MapWaypoint[] waypoints)
            : base(position, spriteKey, acceleration, maxVelocity, stopTime, Level.FORWARD_AND_BACKWARD, waypoints)
        {
            this.deactivated = false;
            this.offScreenReset = offScreenReset;

            this.ResetCallback = new TimerCallback(off_screen_reset_callback);
        }

        // Public Methods ----------------------------------------------------------------------------- Public Methods

        public override void Collide(GameActor actor)
        {
            if (!this.IsStarted())
            {
                Player player = actor as Player;
                if (player != null)
                {
                    this.Start();
                }
            }

            base.Collide(actor);
        }

        public override void Update(GameTime gameTime)
        {
            if (!this.deactivated)
            {
                base.Update(gameTime);
            }
            else if (this.position == this.waypointIterator.PeekLast().Position)
            {
                if (!Camera.GetInstance().IsActorOnScreen(this) && this.offScreenReset)
                {
                    if (!TimerManager.GetInstance().IsTimerRegistered(this.ResetCallback))
                    {
                        TimerManager.GetInstance().RegisterTimer(RESET_DELAY, this.ResetCallback, null);
                    }
                }
                else
                {
                    if (TimerManager.GetInstance().IsTimerRegistered(this.ResetCallback))
                    {
                        TimerManager.GetInstance().UnregisterTimer(this.ResetCallback);
                    }
                }
            }
        }

        // Protected Methods ----------------------------------------------------------------------- Protected Methods

        protected override void OnWaypointReached()
        {
            // If this is the last waypoint, stop.
            if (this.waypointIterator.Current() == this.waypointIterator.PeekLast())
            {
                this.Stop();
                this.deactivated = true;
            }

            base.OnWaypointReached();
        }

        // Private Methods --------------------------------------------------------------------------- Private Methods

        private void off_screen_reset_callback(Object param)
        {
            this.waypointIterator.First();
            this.position = this.waypointIterator.Current().Position;
            this.Stop();
            this.deactivated = false;
        }

    }
}
