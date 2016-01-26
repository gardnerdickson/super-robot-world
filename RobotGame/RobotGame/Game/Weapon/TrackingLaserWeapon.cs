using System;
using Microsoft.Xna.Framework;

namespace RobotGame.Game.Weapon
{
    class TrackingLaserWeapon : LaserWeapon
    {
        // Constants ---------------------------------------------------------------------------------------- Contants

        // Data Members --------------------------------------------------------------------------------- Data Members

        // Properties ------------------------------------------------------------------------------------- Properties

        // Contructors ----------------------------------------------------------------------------------- Contructors

        public TrackingLaserWeapon(int damage, bool collideWithMovers)
            : base(damage, null, null, collideWithMovers)
        { }

        // Public Methods ----------------------------------------------------------------------------- Public Methods

        public override int TryFire(int ammo)
        {
            if (this.enabled)
            {
                if (this.laser == null)
                {
                    this.laser = (TrackingLaser)this.projectileFactory.CreateProjectile(this.position, this.direction);
                }
            }

            return -1;
        }

        public override object Clone()
        {
            return new TrackingLaserWeapon(((TrackingLaserFactory)this.projectileFactory).Damage, ((TrackingLaserFactory)this.projectileFactory).CollideWithMovers);
        }

        // Protected Methods ----------------------------------------------------------------------- Protected Methods

        // Private Methods --------------------------------------------------------------------------- Private Methods
    }
}
