using System;
using Microsoft.Xna.Framework;

namespace RobotGame.Game.Weapon
{
    class HomingMissileFactory : ProjectileFactory
    {
        // Constants ---------------------------------------------------------------------------------------- Contants

        // Data Members --------------------------------------------------------------------------------- Data Members

        private int blastDamage;
        private int blastRadius;
        private int targetRadius;
        private ProjectileSource projectileSource;

        // Properties ------------------------------------------------------------------------------------- Properties

        // Contructors ----------------------------------------------------------------------------------- Contructors

        public HomingMissileFactory(int blastDamage, int blastRadius, int targetRadius, ProjectileSource projectileSource)
        {
            this.blastDamage = blastDamage;
            this.blastRadius = blastRadius;
            this.targetRadius = targetRadius;
            this.projectileSource = projectileSource;
        }

        // Public Methods ----------------------------------------------------------------------------- Public Methods

        public GameActor CreateProjectile(Vector2 position, Vector2 velocity)
        {
            return new HomingMissile(this.blastDamage, this.blastRadius, this.targetRadius, position, velocity, this.projectileSource);
        }

        // Protected Methods ----------------------------------------------------------------------- Protected Methods

        // Private Methods --------------------------------------------------------------------------- Private Methods
    }
}
