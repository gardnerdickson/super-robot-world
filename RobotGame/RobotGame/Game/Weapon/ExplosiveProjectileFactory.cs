using System;
using Microsoft.Xna.Framework;
using RobotGame.Engine;

namespace RobotGame.Game.Weapon
{
    class ExplosiveProjectileFactory : ProjectileFactory
    {
        // Constants ---------------------------------------------------------------------------------------- Contants

        // Data Members --------------------------------------------------------------------------------- Data Members

        private int blastDamage;
        private int blastRadius;
        private float mass;
        private float gravityForce;
        private ProjectileSource projectileSource;
        private float rotationIncrement;
        private SpriteKey spriteKey;

        // Properties ------------------------------------------------------------------------------------- Properties

        // Contructors ----------------------------------------------------------------------------------- Contructors

        public ExplosiveProjectileFactory(int blastDamage, int blastRadius, float mass, float gravityForce, float rotationIncrement, SpriteKey spriteKey, ProjectileSource projectileSource)
        {
            this.blastDamage = blastDamage;
            this.blastRadius = blastRadius;
            this.mass = mass;
            this.gravityForce = gravityForce;
            this.projectileSource = projectileSource;
            this.rotationIncrement = rotationIncrement;
            this.spriteKey = spriteKey;
        }

        // Public Methods ----------------------------------------------------------------------------- Public Methods

        public GameActor CreateProjectile(Vector2 position, Vector2 velocity)
        {
            return new ExplosiveProjectile(this.blastDamage, this.blastRadius, position, velocity, PhysicsMode.Gravity, this.mass, this.gravityForce, this.spriteKey, this.rotationIncrement, this.projectileSource);
        }

        // Protected Methods ----------------------------------------------------------------------- Protected Methods

        // Private Methods --------------------------------------------------------------------------- Private Methods
    }
}
