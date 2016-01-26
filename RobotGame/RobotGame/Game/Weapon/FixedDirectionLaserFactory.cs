using System;
using Microsoft.Xna.Framework;
using RobotGame.Engine;

namespace RobotGame.Game.Weapon
{
    class FixedDirectionLaserFactory : ProjectileFactory
    {
        // Constants ---------------------------------------------------------------------------------------- Contants

        // Data Members --------------------------------------------------------------------------------- Data Members

        private int damage;
        private bool collideWithMovers;

        // Properties ------------------------------------------------------------------------------------- Properties

        public int Damage
        {
            get { return this.damage; }
        }

        public bool CollideWithMovers
        {
            get { return this.collideWithMovers; }
        }
        
        // Contructors ----------------------------------------------------------------------------------- Contructors

        public FixedDirectionLaserFactory(int damage, bool collideWithMovers)
        {
            this.damage = damage;
            this.collideWithMovers = collideWithMovers;
        }

        // Public Methods ----------------------------------------------------------------------------- Public Methods

        public GameActor CreateProjectile(Vector2 position, Vector2 velocity)
        {
            return new FixedDirectionLaser(this.damage, position, velocity, collideWithMovers);
        }

        // Protected Methods ----------------------------------------------------------------------- Protected Methods

        // Private Methods --------------------------------------------------------------------------- Private Methods
    }
}
