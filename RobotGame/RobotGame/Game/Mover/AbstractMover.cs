using System;
using RobotGame.Engine;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Input;
using RobotGame.Game.Waypoint;

namespace RobotGame.Game.Mover
{
    abstract class AbstractMover : GameActor
    {
        enum AccelerationMode
        {
            Accelerate,
            Decelerate
        }

        // Constants ---------------------------------------------------------------------------------------- Contants

        private const float DRAW_DEPTH = Config.MOVER_DRAW_DEPTH;

        // Data Members --------------------------------------------------------------------------------- Data Members

        protected float stopTime;

        protected WaypointIterator waypointIterator;


        private static List<GameActor> moverList = new List<GameActor>();

        private float acceleration;
        private float maxVelocity;
        private float accelerationDistance;

        private Vector2 direction;
        private Vector2 decelerationPoint;

        private AccelerationMode accelerationMode;

        private bool stopped = true;

        // Properties ------------------------------------------------------------------------------------- Properties

        public static List<GameActor> MoverList
        {
            get { return moverList; }
        }

        // Contructors ----------------------------------------------------------------------------------- Contructors

        public AbstractMover(Vector2 position, SpriteKey spriteKey, float acceleration, float maxVelocity,
                     float stopTime, string iterationMode, params MapWaypoint[] waypoints)
            : base(position)
        {
            this.sprite = new Sprite(spriteKey);

            this.acceleration = acceleration;
            this.maxVelocity = maxVelocity;
            this.stopTime = stopTime;

            if (iterationMode == Level.FORWARD_AND_BACKWARD)
            {
                this.waypointIterator = new ForwardAndBackwardWaypointIterator(waypoints);
            }
            else
            {
                this.waypointIterator = new CircularWaypointIterator(waypoints);
            }

            this.position = this.waypointIterator.Current().Position;

            this.drawDepth = DRAW_DEPTH;

            this.accelerationDistance = CalculateDistanceToMaxVelocity(this.acceleration, this.maxVelocity);

            moverList.Add(this);
        }

        // Public Methods ----------------------------------------------------------------------------- Public Methods

        public override void Update(GameTime gameTime)
        {
            if (!stopped)
            {
                if (this.waypointIterator.Current().PassedWaypoint(this.position, this.direction))
                {
                    OnWaypointReached();
                    RecalculateWaypoint();
                }

                // Check if we need to start decelerating
                if (this.accelerationMode == AccelerationMode.Accelerate)
                {
                    if (Vector2.Distance(this.position, this.waypointIterator.Current().Position) <
                        Vector2.Distance(this.decelerationPoint, this.waypointIterator.Current().Position))
                    {
                        this.accelerationMode = AccelerationMode.Decelerate;
                    }
                }

                this.direction = Vector2.Normalize(this.waypointIterator.Current().Position - this.position);

                // Speed up or slow down
                if (this.accelerationMode == AccelerationMode.Accelerate)
                {
                    this.velocity += this.direction * this.acceleration;
                }
                else
                {
                    this.velocity -= this.direction * this.acceleration;
                }

                // Clamp velocity
                if (this.velocity.Length() >= this.maxVelocity)
                {
                    this.velocity = this.direction * this.maxVelocity;
                }

                Move(this.velocity);

                base.Update(gameTime);
            }
        }

        // Protected Methods ----------------------------------------------------------------------- Protected Methods

        protected void Start()
        {
            this.stopped = false;
        }

        protected void Stop()
        {
            this.stopped = true;
        }

        protected bool IsStarted()
        {
            return !this.stopped;
        }

        protected virtual void OnWaypointReached()
        {
            this.position = this.waypointIterator.Current().Position;
        }

        // Private Methods --------------------------------------------------------------------------- Private Methods

        private float CalculateDistanceToMaxVelocity(float acceleration, float maxVelocity)
        {
            // d = v^2 / 2a
            float distance = (float)(Math.Pow(maxVelocity, 2)) / (2 * acceleration);
            return distance;
        }

        private void RecalculateWaypoint()
        {
            // Snap to the current waypoint in case we passed it.
            this.position = this.waypointIterator.Current().Position;
            this.velocity = Vector2.Zero;

            // Iterate to the next waypoint in the list
            this.waypointIterator.Next();

            this.direction = Vector2.Normalize(this.waypointIterator.Current().Position - this.position);

            // Figure out how far we are going to travel while accerating and decelerating
            float distanceToWaypoint = Vector2.Distance(this.position, this.waypointIterator.Current().Position);
            float distanceToDecelerationPoint = distanceToWaypoint - accelerationDistance;
            this.decelerationPoint = this.position + (this.direction * distanceToDecelerationPoint);

            this.accelerationMode = AccelerationMode.Accelerate;
        }
    }
}
