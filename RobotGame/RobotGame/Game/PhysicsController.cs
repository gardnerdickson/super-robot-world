using System;
using Microsoft.Xna.Framework;
using RobotGame.Engine;

namespace RobotGame.Game
{
    public class PhysicsController
    {
        // Constants ---------------------------------------------------------------------------------------- Contants

        // Data Members --------------------------------------------------------------------------------- Data Members

        private float gravityForce;

        private float maxHorizontalVelocity;
        private float maxVerticalVelocity;

        // Properties ------------------------------------------------------------------------------------- Properties

        public float GravityForce
        {
            set { this.gravityForce = value; }
        }

        // Contructors ----------------------------------------------------------------------------------- Contructors

        public PhysicsController(float maxHorizontalVelocity, float maxVerticalVelocity)
        {
            this.maxHorizontalVelocity = maxHorizontalVelocity;
            this.maxVerticalVelocity = maxVerticalVelocity;
        }

        // Public Methods ----------------------------------------------------------------------------- Public Methods

        public Vector2 ApplyGravity(float mass, Vector2 force)
        {
            Vector2 totalForce = force;
            Vector2 outVelocity = Vector2.Zero;

            // Force due to gravity
            totalForce.Y += this.gravityForce;

            if (mass > 0)
            {
                outVelocity = (totalForce / mass);
            }

            // Clamp the resulting velocity
            if (this.maxVerticalVelocity != 0)
            {
                outVelocity.Y = MathHelper.Clamp(outVelocity.Y, -this.maxVerticalVelocity, this.maxVerticalVelocity);
            }
            return outVelocity;
        }

        // Protected Methods ----------------------------------------------------------------------- Protected Methods

        // Private Methods --------------------------------------------------------------------------- Private Methods
    }
}
