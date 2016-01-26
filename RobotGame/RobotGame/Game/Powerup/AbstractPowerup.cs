using System;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using RobotGame.Game;
using RobotGame.Engine;

namespace RobotGame.Game.Powerup
{
    abstract class AbstractPowerup : GameActor
    {
        // Constants ---------------------------------------------------------------------------------------- Contants

        private const float MASS = Config.POWERUP_MASS;

        private const int POINT_VALUE = Config.POWERUP_POINT_VALUE;
        private const float POWERUP_DRAW_DEPTH = Config.POWERUP_DRAW_DEPTH;
        private const float MAX_VELOCITY_Y = Config.POWERUP_MAX_Y_VELOCITY;
        private const float GRAVITY_FORCE = Config.POWERUP_GRAVITY_FORCE;

        // Data Members --------------------------------------------------------------------------------- Data Members

        private static List<GameActor> powerupList = new List<GameActor>();

        // Properties ------------------------------------------------------------------------------------- Properties

        public static List<GameActor> PowerupList
        {
            get { return powerupList; }
        }

        // Contructors ----------------------------------------------------------------------------------- Contructors

        public AbstractPowerup(Vector2 position, Vector2 velocity, PhysicsMode physicsMode)
            : base(position, velocity, physicsMode)
        {
            this.mass = MASS;
            this.drawDepth = POWERUP_DRAW_DEPTH;

            this.physicsController.GravityForce = GRAVITY_FORCE;

            powerupList.Add(this);
        }

        // Public Methods ----------------------------------------------------------------------------- Public Methods

        public override void Collide(GameActor actor)
        {
            this.ApplyPowerup((Player)actor);
            ((Player)actor).Points += POINT_VALUE;
            this.Remove();
        }

        public override void Update(GameTime gameTime)
        {
            this.ApplyPhysics();
            this.velocity.Y = MathHelper.Clamp(this.velocity.Y, -MAX_VELOCITY_Y, MAX_VELOCITY_Y);
            this.Move(this.velocity);

            if (Level.GetInstance().IsBoundsOnMap(this.Bounds))
            {
                this.HandleMapCollisions();
                this.HandleMoverCollisions();
            }
            else
            {
                this.Remove();
            }

            base.Update(gameTime);
        }

        // Protected Methods ----------------------------------------------------------------------- Protected Methods

        protected abstract void ApplyPowerup(Player player);

        // Private Methods --------------------------------------------------------------------------- Private Methods
    }
}
